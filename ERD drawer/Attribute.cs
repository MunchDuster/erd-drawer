using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ERD_drawer
{
    [Serializable]
    public class Attribute : Displayable
    {
        [JsonIgnore] public static readonly Color color = Color.DarkGreen;
        [JsonIgnore] protected override Color Color => color;

        [JsonIgnore] public AttributedDisplayable parent;
        public bool isKey { get; set; }

        public Attribute(string name, int x, int y, AttributedDisplayable parent, bool isKey) : base(name, x, y)
        {
            this.isKey = isKey;
            this.parent = parent;
        }

        [JsonConstructor]
        public Attribute(string name, int x, int y, bool isKey, int id) : base(name, x, y, id)
        {
            this.isKey = isKey;
        }

        public override void Draw(bool selected)
        {
            if (parent == null)
                return;

            paper.DrawEllipse(pen, X, Y, Width, Height);

            if (selected)
                paper.FillEllipse(new SolidBrush(Color), X, Y, Width, Height);

            DrawName(isKey);

            
            SplitLine.DrawVerticalCornerLine(parent, this);
        }

        public override void Delete()
        {
            base.Delete();

            if(parent == null)
            {
                Debug.WriteLine("Attribute has no parent to remove from!");
                return;
            }
            parent.Attributes.Remove(this);
        }
    }
}
