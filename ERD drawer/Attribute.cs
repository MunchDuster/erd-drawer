using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ERD_drawer
{
    [Serializable]
    public class Attribute : Displayable
    {
        [JsonIgnore] public static readonly Color color = Color.DarkGreen;
        [JsonIgnore] protected override Color Color => color;

        public bool isKey { get; set; }

        public Attribute(string name, int x, int y, bool isKey) : base(name, x, y)
        {
            this.isKey = isKey;
        }

        [JsonConstructor]
        public Attribute(string name, int x, int y, bool isKey, int id) : base(name, x, y, id)
        {
            this.isKey = isKey;
        }

        public override void Draw(bool selected)
        {
            paper.DrawEllipse(pen, X, Y, Width, Height);

            if (selected)
                paper.FillEllipse(new SolidBrush(Color), X, Y, Width, Height);

            DrawName(isKey);
        }

        public override void Delete()
        {
            base.Delete();

            AttributedDisplayable? parent = Filter<AttributedDisplayable>().FirstOrDefault(attributable => attributable.Attributes.Contains(this));
            if(parent == null)
            {
                Debug.WriteLine("Attribute has no parent to remove from!");
                return;
            }
            parent.Attributes.Remove(this);
        }
    }
}
