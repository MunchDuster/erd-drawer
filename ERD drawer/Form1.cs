using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ERD_drawer
{
    public partial class Form1 : Form
    {
        private string fileName;

        private Data data;

        public static List<int> selected = new();

        // for moving around the world 🌍
        private bool worldDragging = false;
        private bool selectedDragging = false;
        private bool boxSelecting = false;

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
            Displayable.font = new("Consolas", 10);
            Displayable.form1 = this;

            pictureBox1_Paint(new object(), new PaintEventArgs(CreateGraphics(), new Rectangle()));
        }

        #region saving and loading
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Displayable.DeleteAll();
                selected.Clear();

                fileName = openFileDialog1.FileName;

                data = FileSystem.LoadFile(fileName);


                Entity.Entities = data.Entities;
                Relationship.Relationships = data.Relationships;

                Relationship.PopulateMap();
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
        #endregion
        #region drawing
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Displayable.paper = e.Graphics;

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

            if (mouseEvent.Button != MouseButtons.Left)
                return;

            Displayable clicked = CheckCollisions(mouseEvent.X, mouseEvent.Y);
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
            boxSelecting = false;
            selectedDragging = false;
            worldDragging = false;

            selected = selected.Concat(tempBoxSelected).ToList();

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
                tempBoxSelected.Clear();

                foreach (Displayable hightlighted in CheckCollisions(GetBoxSelect()))
                {
                    if (!tempBoxSelected.Contains(hightlighted.Id))
                    {
                        tempBoxSelected.Add(hightlighted.Id);
                        //hightlighted.IsSelected = true;
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

            string[] inputs = { "Name" };
            string[] outputs = Prompt.ShowDialog(inputs, "New Entity", Entity.color);

            if (outputs == null)
                return;

            Select(new Entity(outputs[0], new(), x, y));
        }

        private void relationshipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected.Count != 2)
            {
                MessageBox.Show("Must have exactly 2 entities selected before making a relationship.");
                return;
            }

            // Swap indices otherwise left is right and right is left
            Entity entity1 = Entity.Entities.First(e => e.Id == selected[0]);
            Entity entity2 = Entity.Entities.First(e => e.Id == selected[1]);

            if (entity1 == null || entity2 == null)
            {
                MessageBox.Show("Must have exactly 2 entities selected before making a relationship.");
                return;
            }

            int x = (entity1.X + entity2.X) / 2;
            int y = (entity1.Y + entity2.Y) / 2;

            string[] inputs = { "Name", "L Quantity", "R Quantity" };
            string[] outputs = Prompt.ShowDialog(inputs, "New Relationship", Relationship.color);

            if (outputs == null)
                return;

            bool entity1IsLeft = entity1.X < entity2.X;
            Entity leftEntity = entity1IsLeft ? entity1 : entity2;
            Entity rightEntity = entity1IsLeft ? entity2 : entity1;
            Select(new Relationship(outputs[0], new(), x, y, leftEntity.Name, rightEntity.Name, outputs[1], outputs[2], 0.5));
        }

        private void attributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] inputs = { "Name" };
            string[] outputs = Prompt.ShowDialog(inputs, "New Attribute", Attribute.color);

            if (outputs == null)
                return;

            foreach (AttributedDisplayable selected in Filter<AttributedDisplayable>(selected))
                selected.AddAttribute(outputs[0]);
        }
        #endregion
        #region other menu strip buttons

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int antiInfinityLoop = 0; antiInfinityLoop < 1000 && selected.Count > 0; antiInfinityLoop++)
            {
                Displayable.Displayables.Find(d => d.Id == selected[0]).Delete();
                selected.RemoveAt(0);
            }
        }

        private void setAsKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Attribute attribute in Filter<Attribute>(selected))
                attribute.isKey = true;
        }

        private void setAsNotKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Attribute attribute in Filter<Attribute>(selected))
                attribute.isKey = false;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] inputs = { "New Name" };
            string[] outputs = Prompt.ShowDialog(inputs, "Rename", Color.White);

            if (outputs == null)
                return;

            foreach (int displayable in selected)
                Displayable.Displayables.Find(d => selected.Contains(displayable)).Name = outputs[0];
        }

        private void allToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            selected.Clear();
            foreach (Displayable selectedItem in Displayable.Displayables)
                selected.Add(selectedItem.Id);
        }

        private void changeQuantityLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Relationship> relationships = Filter<Relationship>(selected);

            if (relationships.Count == 0)
            {
                MessageBox.Show("No relationships to change quantities!");
                return;
            }

            string[] inputs = { "L Bounds", "R Bounds" };
            string[] outputs = Prompt.ShowDialog(inputs, "Rename", Color.White);

            if (outputs == null)
                return;

            foreach (Relationship relationship in relationships)
            {
                relationship.LeftQuantity = outputs[0];
                relationship.RightQuantity = outputs[1];
            }
        }
        #endregion

        private List<T> Filter<T>(List<int> displayableIds) where T : Displayable
        {
            return Displayable.Displayables.Where(displayable => displayableIds.Contains(displayable.Id)).Select(d => d as T).Where(s => s != null).Select(d => d!).ToList();
        }

        private void Select(Displayable displayable)
        {
            selected.Clear();
            selected.Add(displayable.Id);
        }

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

        private void saveAsjpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveImageFileDialog.ShowDialog() != DialogResult.OK)
                return;
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bitmap, pictureBox1.ClientRectangle);

            pictureBox1.Image = bitmap;  //this makes your changes visible

            string path = saveImageFileDialog.FileName;
            pictureBox1.Image.Save(path, ImageFormat.Jpeg);

            pictureBox1.Image = null;
        }
    }
}
