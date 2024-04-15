using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ERD_drawer
{
    public abstract class AttributedDisplayable : Displayable
    {
        [JsonIgnore] private const int attributeSpacing = 5;
        [JsonIgnore] private const int shiftLeftFromParent = 20;

        public List<Attribute> Attributes;

        public AttributedDisplayable(string name, List<Attribute> attributes, int x, int y) : base(name, x, y)
        {
            Attributes = attributes;
        }
        public AttributedDisplayable(string name, List<Attribute> attributes, int x, int y, int id) : base(name, x, y, id)
        {
            Attributes = attributes;
        }

        protected void DrawAttributes(bool selected)
        {
            foreach (Attribute attribute in Attributes)
            {
                attribute.Draw(attribute.IsSelected);
                SplitLine.DrawVerticalCornerLine(this, attribute);
            }
        }

        public void AddAttribute(string name)
        {
            if(Attributes.Any(attribute => attribute.Name == name))
            {
                MessageBox.Show("Already added an attribute with same name");
                return;
            }

            Attribute attribute = new Attribute(name, 100, 100, false); // need to make the attribute first to get its width and height to then place it
            attribute.X = X - shiftLeftFromParent - attribute.Width;
            attribute.Y = Y + (Attributes.Count + 1) * (attribute.Height + attributeSpacing);
            Attributes.Add(attribute);
        }

        public override void Delete()
        {
            base.Delete();
            for (int antiInfinityLoop = 0; antiInfinityLoop < 1000 && Attributes.Count > 0; antiInfinityLoop++)
            {
                Attributes[0].Delete();
            }
        }
    }
}
