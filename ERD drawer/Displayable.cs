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
        [JsonIgnore] public virtual int Height => 20 + Settings.fontHeightPerChar;

        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        [JsonIgnore] public int Right => X + Width;
        [JsonIgnore] public int Left => X;
        [JsonIgnore] public int Top => Y;
        [JsonIgnore] public int Bottom => Y + Height;
        [JsonIgnore] public int MiddleX => X + Width / 2;
        [JsonIgnore] public int MiddleY => Y + Height / 2;
        [JsonIgnore] public Point Center => new(MiddleX, MiddleY);

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

        public static Displayable Find(int id) => Displayables.First(x => x.Id == id);

        public abstract void Draw(bool selected);

        protected void DrawName(bool underlined = false)
        {
            Font? chosenFont = underlined ? new Font(font, FontStyle.Underline) : font;
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
            return Settings.fontWidthPerChar * text.Length; // haha monospace font go brrr
        }

        public virtual void Delete()
        {
            Displayables.Remove(this);
            Form1.selected.Remove(Id);
        }

        public static void DeleteAll()
        {
            for (int antiInfinityLoop = 0; antiInfinityLoop < 1000 && Displayables.Count > 0; antiInfinityLoop++)
            {
                Displayables[0].Delete();
            }
        }

        public static List<T> Filter<T>(List<int>? displayableIds = null) where T : Displayable
        {
            List<T> values = new();

            foreach(Displayable displayable in Displayables)
                if (displayable is T child && (displayableIds == null || displayableIds.Contains(displayable.Id)))
                    values.Add(child);

            return values;
        }
    }
}
