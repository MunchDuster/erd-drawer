using System.Diagnostics;
using System.Drawing.Imaging;

namespace ERD_drawer
{
    public partial class Form1 : Form
    {
        private const string emptyQauntity = "()";
        private string? fileName;

        private Data? data;

        public static List<int> selected = new();
        public static bool ShiftPresed => (ModifierKeys & Keys.Shift) == Keys.Shift;

        // for moving around the world
        private bool worldDragging = false;
        private bool selectedDragging = false;

        // for checking whether box selecting
        private bool boxSelecting = false;
        private bool hasActuallyStartBoxSelecting = false; // to differentiate between box selecting and a click

        // for box selecting
        private static readonly Color boxSelectFill = Color.FromArgb(50, 0, 0, 255);
        private static readonly Color boxSelectBorder = Color.Blue;
        private int boxCornerX;
        private int boxCornerY;
        private List<int> tempBoxSelected = new();

        // for selecting & dragging
        private int lastX;
        private int lastY;

        public Form1()
        {
            InitializeComponent();
            Displayable.form1 = this;
            Settings.Apply();
            pictureBox1_Paint(new object(), new PaintEventArgs(CreateGraphics(), new()));
        }

        #region saving and loading
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Displayable.DeleteAll();
                selected.Clear();

                fileName = openFileDialog1.FileName;

                Data? loadedData = null;

                if (!FileSystem.LoadFile(fileName, ref loadedData))
                    return;

                if (loadedData == null)
                    return;

                data = loadedData;

                Entity.Entities = data.Entities;
                Relationship.Relationships = data.Relationships;

                Relationship.PopulateMap();

                MoveToCentre();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;

                data = new();
                data.Entities = Entity.Entities;
                data.Relationships = Relationship.Relationships;

                FileSystem.SaveFile(fileName, data);
            }
        }

        private void saveAsJpgToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveImage(ImageFormat.Jpeg);
        }
        private void saveAsPngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveImage(ImageFormat.Png);
        }

        private void SaveImage(ImageFormat format)
        {
            if (saveImageFileDialog.ShowDialog() != DialogResult.OK)
                return;

            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bitmap, pictureBox1.ClientRectangle);

            pictureBox1.Image = bitmap;  //this makes your changes visible

            string path = saveImageFileDialog.FileName;
            pictureBox1.Image.Save(path, format);

            pictureBox1.Image = null;
        }

        #endregion
        #region drawing
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Displayable.paper = e.Graphics;
            SplitLine.paper = e.Graphics;

            if (boxSelecting)
            {
                Rectangle boxSelect = GetBoxSelect();
                e.Graphics.FillRectangle(new SolidBrush(boxSelectFill), boxSelect);
                e.Graphics.DrawRectangle(new Pen(boxSelectFill), boxSelect);
            }

            foreach (Entity displayable in Entity.Entities)
            {

                displayable.Draw(displayable.IsSelected);
            }

            foreach (Relationship displayable in Relationship.Relationships)
                displayable.Draw(displayable.IsSelected);
        }

        //private void DrawGrid(PaintEventArgs e)
        //{
        //    Graphics paper = e.Graphics;

        //    const int lineGap = 30;
        //    Pen pen = new Pen(Color.CadetBlue, 1);

        //    int xOffset = offsetX % lineGap;
        //    int yOffset = offsetY % lineGap;

        //    for (int x = xOffset; x < pictureBox1.Width; x += lineGap)
        //    {
        //        paper.DrawLine(pen, new Point(x, 0), new Point(x, pictureBox1.Height));
        //    }
        //    for (int y = yOffset; y < pictureBox1.Height; y += lineGap)
        //    {
        //        paper.DrawLine(pen, new Point(0, y), new Point(pictureBox1.Width, y));
        //    }
        //}
        #endregion
        #region picture box clicking
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseEvent = (MouseEventArgs)e;


            if (mouseEvent == null)
                return;

            if (mouseEvent.Button != MouseButtons.Left)
                return;

            Displayable? clicked = CheckCollisions(mouseEvent.X, mouseEvent.Y);
            Debug.WriteLine(clicked == null ? "nothing" : clicked.Name);


            if (clicked == null)
            {
                selected.Clear();
                return;
            }

            if (selected.Contains(clicked.Id))
            {
                selected.Remove(clicked.Id);
                return;
            }

            if (!ShiftPresed)
            {
                selected.Clear();
            }

            selected.Add(clicked.Id);
        }

        #endregion
        #region picture box dragging

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                worldDragging = true;
                lastX = e.X;
                lastY = e.Y;
                return;
            }

            bool clickingonDisplayable = CheckCollisions(e.X, e.Y) != null;

            selectedDragging = clickingonDisplayable;
            boxSelecting = !clickingonDisplayable;

            if (boxSelecting)
            {
                boxCornerX = e.X;
                boxCornerY = e.Y;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (hasActuallyStartBoxSelecting && !ShiftPresed)
            {
                selected.Clear();
            }
            
            boxSelecting = false;
            selectedDragging = false;
            worldDragging = false;
            hasActuallyStartBoxSelecting = false;

            foreach (int selectedID in tempBoxSelected)
            {
                if (!selected.Contains(selectedID))
                {
                    selected.Add(selectedID);
                }
            }
            tempBoxSelected.Clear();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int deltaX = e.X - lastX;
            int deltaY = e.Y - lastY;


            if (worldDragging || selectedDragging)
            {
                List<Displayable> moving = worldDragging
                    ? Displayable.Displayables
                    : Displayable.Displayables.Where(d => selected.Contains(d.Id)).ToList();

                foreach (Displayable displayable in moving)
                {
                    displayable.X += deltaX;
                    displayable.Y += deltaY;
                }
            }
            else if (boxSelecting)
            {
                hasActuallyStartBoxSelecting = Math.Abs(boxCornerX - e.X) + Math.Abs(boxCornerY - e.Y) > 10; // to differentiate from clicking

                    tempBoxSelected.Clear();

                foreach (Displayable hightlighted in CheckCollisions(GetBoxSelect()))
                {
                    if (!tempBoxSelected.Contains(hightlighted.Id))
                    {
                        tempBoxSelected.Add(hightlighted.Id);
                    }
                }
            }

            lastX += deltaX;
            lastY += deltaY;
        }

        private Rectangle GetBoxSelect()
        {
            int x = boxCornerX < lastX ? boxCornerX : lastX;
            int y = boxCornerY < lastY ? boxCornerY : lastY;
            int width = Math.Abs(lastX - boxCornerX);
            int height = Math.Abs(lastY - boxCornerY);
            return new(x, y, width, height);
        }

        #endregion
        #region creating
        private void entityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = 100;
            const int y = 200;

            // Because I got so damn bored of needing to move the second entity while testing
            while (Displayable.Displayables.Any(displayable => displayable.CheckCollision(x, y)))
            {
                x += 500;
            }

            Dictionary<string, string> inputs = new() { { "Name", "" } };
            string[]? outputs = Prompt.ShowDialog(inputs, "New Entity", Entity.color);

            if (outputs == null)
                return;

            Select(new Entity(outputs[0], new(), x, y));
        }

        private void relationshipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Entity> entities = Displayable.Filter<Entity>(selected);

            if (entities.Count == 0)
            {
                MessageBox.Show("Must have at least 1 entity selected before making a relationship.");
                return;
            }

            int x = 0, y = 0;

            bool isUnary = entities.Count == 1;

            if (isUnary)
            {
                x = entities[0].Right + 100;
                y = entities[0].Y;
            }
            else
            {
                x = entities.Sum(entity => entity.X) / entities.Count;
                y = entities.Sum(entity => entity.Y) / entities.Count;
            }

            Dictionary<string, string> inputs = new() { { "Name", string.Empty } };

            if (isUnary)
            {
                inputs.Add(entities[0].Name + "'s Quantity In", emptyQauntity);
                inputs.Add(entities[0].Name + "'s Quantity Out", emptyQauntity);
            }
            else
            {
                string[] names = entities.Select(entity => entity.Name + "'s Quantity").ToArray();
                foreach (string name in names)
                {
                    inputs.Add(name, emptyQauntity);
                }
            }

            string[]? outputs = Prompt.ShowDialog(inputs, "New Relationship", Relationship.color);

            if (outputs == null)
                return; // cancelled creation prompt

            List<Relationship.RelationshipLink> links = new();

            if (isUnary)
            {
                links.Add(new(entities[0].Id, outputs[1]));
                links.Add(new(entities[0].Id, outputs[2]));
            }
            else
            {
                Debug.WriteLine($"outputs: " + outputs.Length + ", inputs: " + inputs.Count + ", entities: " + entities.Count);
                for (int i = 0; i < entities.Count; i++)
                {
                    links.Add(new(entities[i].Id, outputs[i + 1]));
                }
            }

            Select(new Relationship(outputs[0], new(), x, y, links, 0.5, isUnary));
        }

        private void attributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> inputs = new() { { "Name", string.Empty } };
            string[]? outputs = Prompt.ShowDialog(inputs, "New Attribute", Attribute.color);

            if (outputs == null)
                return;

            string attribute = outputs[0];
            List<AttributedDisplayable> displayables = Displayable.Filter<AttributedDisplayable>(selected);

            foreach (AttributedDisplayable selected in displayables)
                selected.AddAttribute(attribute);
        }
        #endregion
        #region other menu strip buttons
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int antiInfinityLoop = 0; antiInfinityLoop < 1000 && selected.Count > 0; antiInfinityLoop++)
            {
                Displayable? selectedD = Displayable.Displayables.Find(d => d != null && d.Id == selected[0]);

                if (selectedD == null)
                    continue;

                selectedD.Delete();
            }
        }

        private void setAsKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Attribute attribute in Displayable.Filter<Attribute>(selected))
                attribute.isKey = true;
        }

        private void setAsNotKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Attribute attribute in Displayable.Filter<Attribute>(selected))
                attribute.isKey = false;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string placeholder = selected.Count == 1 ? Displayable.Find(selected[0]).Name : string.Empty;
            Dictionary<string, string> inputs = new() { { "New Name", placeholder } };
            string[]? outputs = Prompt.ShowDialog(inputs, "Rename", Color.White);

            if (outputs == null)
                return;

            foreach (int id in selected)
                Displayable.Find(id).Name = outputs[0];
        }

        private void allToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            selected.Clear();
            foreach (Displayable selectedItem in Displayable.Displayables)
                selected.Add(selectedItem.Id);
        }

        private void changeQuantityLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Relationship> relationships = Displayable.Filter<Relationship>(selected);

            if (relationships.Count == 0)
            {
                MessageBox.Show("No relationships selected to change quantities!");
                return;
            }

            foreach (Relationship relationship in relationships)
            {
                Dictionary<string, string> inputs = relationship.Links.Select(link => new KeyValuePair<string, string>(link.Entity.Name, link.Quantity)).ToDictionary();
                string[]? outputs = Prompt.ShowDialog(inputs, "Change Quantities", Color.White);

                if (outputs == null)
                    return;

                for (int i = 0; i < relationship.Links.Count; i++)
                    relationship.Links[i].Quantity = outputs[i];
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected.Clear();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This is an entity-relationship diagram (ERD) maker app." +
                "\nMade by [Unimportant] for COMPX223." +
                "\nThis allows for easy creation and editing of ERDs" +
                "\nNote: Can only edit the json files of previously made ERDs" +
                "\nPractice the shortcuts to improve your diagram-making efficiency." +
                "\nIf there are any issues then let me know; " +
                "\nOr maybe you can try fix it yourself ;)" +
                "\nI'm on the COMPX discord as MunchDuster." +
                "\nHave fun!"
            );
        }
        #endregion

        #region collision detection

        private Displayable? CheckCollisions(int x, int y)
        {
            foreach (Displayable displayable in Displayable.Displayables)
                if (displayable.CheckCollision(x, y))
                    return displayable;

            return null;
        }

        private List<Displayable> CheckCollisions(Rectangle area)
        {
            List<Displayable> selected = new();

            foreach (Displayable displayable in Displayable.Displayables)
                if (displayable.CheckCollision(area))
                    selected.Add(displayable);

            return selected;
        }
        #endregion

        private void Select(Displayable displayable)
        {
            selected.Clear();
            selected.Add(displayable.Id);
        }


        private void MoveToCentre()
        {
            int averageX = 0;
            int averageY = 0;
            foreach (Entity entity in Entity.Entities)
            {
                averageX += entity.X;
                averageY += entity.Y;
            }
            averageX /= Entity.Entities.Count;
            averageY /= Entity.Entities.Count;

            averageX -= pictureBox1.Width / 2;
            averageY -= pictureBox1.Height / 2;

            for (int i = 0; i < Displayable.Displayables.Count; i++)
            {
                Displayable.Displayables[i].X -= averageX;
                Displayable.Displayables[i].Y -= averageY;
            }

            // Scale window to fit if possible
            int idealWidth = Displayable.Displayables.Max(dis => dis.Right) + 70;
            int idealHeight = Displayable.Displayables.Max(dis => dis.Bottom) + 70;
        }

        private void fontFamilyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> inputs = new() { { "Font", Settings.font } };
            string[]? outputs = Prompt.ShowDialog(inputs, "Change Font Family", Color.Black);

            if (outputs == null)
                return;

            Settings.font = outputs[0];
            Settings.Apply();
        }

        private void fontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> inputs = new() { { "Font size", Settings.fontSize.ToString() } };
            string[]? outputs = Prompt.ShowDialog(inputs, "Change Font Size", Color.Black);

            if (outputs == null)
                return;

            if (int.TryParse(outputs[0], out int fontSize))
            {
                Settings.fontSize = fontSize;
                Settings.Apply();
                return;
            }

            MessageBox.Show("Could not parse font size!");
        }

        private void increaseFontSpaceWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.fontWidthPerCharMultiplier += 0.1f;
            Settings.Apply();
        }

        private void increaseFontSpaceHeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.fontHeightPerCharMultiplier += 0.1f;
            Settings.Apply();
        }

        private void decreaseFontSpaceWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.fontWidthPerCharMultiplier -= 0.1f;
            Settings.Apply();
        }

        private void decreaseFontSpaceHeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.fontHeightPerCharMultiplier -= 0.1f;
            Settings.Apply();
        }
    }
}
