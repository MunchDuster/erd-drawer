using System.Text.Json.Serialization;

namespace ERD_drawer
{
    public class Entity : AttributedDisplayable
    {
        public static List<Entity> Entities = new();
        public static readonly Color color = Color.Red;
        [JsonIgnore] protected override Color Color => color;

        [JsonIgnore] protected override int TextTop => 7;

        public Entity(string name, List<Attribute> attributes, int x, int y) : base(name, attributes, x, y)
        {
            Entities.Add(this);
        }

        [JsonConstructor]
        public Entity(string name, List<Attribute> attributes, int x, int y, int id) : base(name, attributes, x, y, id)
        {
            Entities.Add(this);
            foreach (Attribute a in attributes)
                a.parent = this; // Look at me, I am the capytan now
        }

        public override void Draw(bool selected)
        {
            paper.DrawRectangle(pen, X, Y, Width, Height);

            if (selected)
                paper.FillRectangle(new SolidBrush(Color), X, Y, Width, Height);

            DrawName();

            foreach (Attribute attribute in Attributes)
                attribute.Draw(attribute.IsSelected); 
        }

        public override void Delete()
        {
            base.Delete();
            Entities.Remove(this);
            Relationship.RemoveEntity(this);
        }
    }
}
