using System.Text.Json.Serialization;

namespace ERD_drawer
{
    public abstract class Displayable
    {
        [JsonIgnore] public static List<Displayable> Displayables = new();
        [JsonIgnore] private static readonly Random random = new();

        [JsonIgnore] public bool IsSelected => Form1.selected.Contains(Id);

        [JsonIgnore] public static Form1 form1;
        [JsonIgnore] public static Graphics paper;
        [JsonIgnore] public static Font font;
        [JsonIgnore] protected virtual Color Color { get; }
        [JsonIgnore] protected virtual int TextLeft => 10;
        [JsonIgnore] protected virtual int TextTop => 10;

        [JsonIgnore] public int Width => TextLeft * 2 + GetStringWidth(Name);
        [JsonIgnore] public virtual int Height => 40;

        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        [JsonIgnore] protected SolidBrush brush => new (Color.Black);
        [JsonIgnore] private Pen? _pen;
        [JsonIgnore] protected Pen pen
        {
            get
            {
                if (_pen == null)
                    _pen = new Pen(Color, 2);

                return _pen;
            }
        }

        public abstract void Draw(bool selected);

        protected void DrawName(bool underlined = false)
        {
            var chosenFont = underlined ? new Font(font, FontStyle.Underline) : font;
            paper.DrawString(Name, chosenFont, brush, X + TextLeft, Y + TextTop);
        }

        protected Displayable(string name, int x, int y)
        {
            int id = random.Next();
            Displayables.Add(this);
            Name = name;
            X = x;
            Y = y;
            Id = id;
        }
        protected Displayable(string name, int x, int y, int id)
        {
            Id = id == 0 ? random.Next() : id;
            Displayables.Add(this);
            Name = name;
            X = x;
            Y = y;
        }

        public bool CheckCollision(int x,int y)
        {
            return !(x < X || x > X + Width || y < Y || y > Y + Height);
        }
        public bool CheckCollision(Rectangle area)
        {
            return X > area.X && X + Width < area.X + area.Width && Y > area.Y && Y + Height < area.Y + area.Height;
        }

        /// <summary>
        /// Source: https://www.codeproject.com/Articles/2118/Bypass-Graphics-MeasureString-limitations
        /// This exists because unlike p5js there isnt a good C# version of textWidth(text) 
        /// </summary>
        /// <returns></returns>
        public static int GetStringWidth(string text)
        {
            //RectangleF rect = new(0, 0, 2000, 2000);

            //CharacterRange[] ranges = { new(0, text.Length) };
            //StringFormat format = new();
            //format.SetMeasurableCharacterRanges(ranges);

            //Region[] regions = paper.MeasureCharacterRanges(text, font, rect, format);
            //rect = regions[0].GetBounds(paper);

            //return (int)(rect.Right + 1.0f);

            return 12 * text.Length; // haha monospace font go brrr
        }

        public virtual void Delete()
        {
            Displayables.Remove(this);
        }

        public static void DeleteAll()
        {
            for (int antiInfinityLoop = 0; antiInfinityLoop < 1000 && Displayables.Count > 0; antiInfinityLoop++)
            {
                Displayables[0].Delete();
            }
        }

        /// <summary>
        /// Draws line that would be diagonal in three parts such that there are no diagonal lines
        /// </summary>
        protected void DrawSplitLine(Displayable A, Displayable B)
        {
            bool isAOnRight = A.X > B.X;
            Displayable right = isAOnRight ? A : B;
            Displayable left = isAOnRight ? B : A;

            bool isOverTop = left.X < right.X && left.X + left.Width > right.X;

            Point startPoint = new Point(right.X, right.Y + right.Height / 2);
            Point endPoint = new Point(left.X + left.Width, left.Y + left.Height / 2);

            int midX = startPoint.X + (int)((endPoint.X - startPoint.X) * 0.5);

            if (isOverTop)
            {
                bool closerToLeft = Math.Abs(left.X - right.X) < Math.Abs(left.X + left.Width - (right.X + right.Width));
                endPoint.X = closerToLeft ? left.X : left.X + left.Width;
                startPoint.X = !closerToLeft ? right.X + right.Width : right.X;
                if (closerToLeft)
                    midX = Math.Min(left.X, right.X) - 10;
                else
                    midX = Math.Max(left.X + left.Width, right.X + right.Width) + 10;
            }

            Point point2 = new(midX, startPoint.Y);
            Point point3 = new(midX, endPoint.Y);

            paper.DrawLine(pen, startPoint, point2);
            paper.DrawLine(pen, point2, point3);
            paper.DrawLine(pen, point3, endPoint);
        }
    }
}
