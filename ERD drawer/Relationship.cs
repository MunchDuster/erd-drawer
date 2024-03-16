using System.Text.Json.Serialization;

namespace ERD_drawer
{
    public class Relationship : AttributedDisplayable
    {
        [JsonIgnore] const int numberPaddingX = -10;
        [JsonIgnore] const int numberPaddingY = 10;

        [JsonIgnore] public static readonly Color color = Color.Blue;
        [JsonIgnore] protected override Color Color => color;

        [JsonIgnore] public static List<Relationship> Relationships = new();

        [JsonIgnore] public static Dictionary<string, List<Relationship>> RelationshipsMap = new();

        [JsonIgnore] protected override int TextLeft => 15;
        [JsonIgnore] protected override int TextTop => 28;
        [JsonIgnore] public override int Height => 80;

        public string LeftEntityName { get; set; }
        public string RightEntityName { get; set; }
        public string LeftQuantity { get; set; }
        public string RightQuantity { get; set; }

        public double lineBias { get; set; }

        [JsonIgnore] private string key => $"{LeftEntityName}-{RightEntityName}";

        public static void PopulateMap()
        {
            RelationshipsMap.Clear();

            foreach (Relationship relationship in Relationships)
            {
                if (RelationshipsMap.ContainsKey(relationship.key))
                    RelationshipsMap[relationship.key].Add(relationship);
                else
                    RelationshipsMap.Add(relationship.key, new List<Relationship> { relationship });
            }
        }
        public static void RemoveEntity(Entity entity)
        {
            for (int antiInfinityLoop = 0; antiInfinityLoop < 1000 && Relationships.Count > 0; antiInfinityLoop++)
            {
                if (Relationships[0].LeftEntityName == entity.Name || Relationships[0].RightEntityName == entity.Name)
                    Relationships[0].Delete();
            }
            PopulateMap();
        }

        public Relationship(string name, List<Attribute> attributes, int X, int Y, string leftEntityName, string rightEntityName, string leftQuantity, string rightQuantity, double lineBias) : base(name, attributes, X, Y)
        {
            LeftEntityName = leftEntityName;
            RightEntityName = rightEntityName;
            LeftQuantity = leftQuantity;
            RightQuantity = rightQuantity;
            this.lineBias = lineBias;

            form1.MouseWheel += ChangeBias;

            Relationships.Add(this);
            PopulateMap();
        }

        [JsonConstructor]
        public Relationship(string name, List<Attribute> attributes, int X, int Y, string leftEntityName, string rightEntityName, string leftQuantity, string rightQuantity, double lineBias, int id) : base(name, attributes, X, Y, id)
        {
            LeftEntityName = leftEntityName;
            RightEntityName = rightEntityName;
            LeftQuantity = leftQuantity;
            RightQuantity = rightQuantity;
            this.lineBias = lineBias;

            form1.MouseWheel += ChangeBias;

            Relationships.Add(this);
            PopulateMap();
        }

        private void ChangeBias(object? sender, MouseEventArgs e)
        {
            if (IsSelected)
                lineBias = Math.Clamp(lineBias + e.Delta / (5.0 * SystemInformation.MouseWheelScrollDelta), 0, 1);
        }

        public override void Draw(bool selected)
        {
            DrawDiamond();
            DrawLine();
            DrawName();

            foreach (Attribute attribute in Attributes)
                attribute.Draw(attribute.IsSelected);
        }

        private void DrawLine()
        {
            Console.WriteLine("drawing relationship: " + Name);
            Entity leftEntity = Entity.Entities.Find(entity => entity.Name.Equals(LeftEntityName))!;
            Entity rightEntity = Entity.Entities.Find(entity => entity.Name.Equals(RightEntityName))!;

            //int entityX = 0;
            //int entityY = 0;
            //switch (Direction)
            //{
            //    case "left":
            //        entityX = leftEntity.X - (leftEntity + width);
            //        entityY = leftEntity.Y;
            //        break;
            //    case "right":
            //        entityX = startingEntity.X + (entitySpacing + width);
            //        entityY = startingEntity.Y;
            //        break;
            //    case "up":
            //        entityX = startingEntity.X;
            //        entityY = startingEntity.Y - (entityVertSpacing + Height);
            //        break;
            //    case "down":
            //        entityX = startingEntity.X;
            //        entityY = startingEntity.Y + (entityVertSpacing + Height);
            //        break;
            //    default:
            //        Console.WriteLine("Unknown direction " + Direction + "!");
            //        break;
            //}

            DrawSplitLine(leftEntity, this);
            DrawSplitLine(this, rightEntity);

            paper.DrawString(LeftQuantity, font, brush, X - numberPaddingX - GetStringWidth(RightQuantity), Y + numberPaddingY);
            paper.DrawString(RightQuantity, font, brush, X + Width + numberPaddingX, Y + numberPaddingY);
        }

        private void DrawDiamond()
        {
            int xMid = X + Width / 2;
            int xLeft = X;
            int xRight = X + Width;
            int yTop = Y;
            int yMid = Y + Height / 2;
            int yBottom = Y + Height;

            Point[] points =
            {
                new (xLeft, yMid), // left
                new (xMid, yTop), // top
                new (xRight, yMid), // right
                new (xMid, yBottom), // bottom
            };

            paper.DrawPolygon(pen, points);

            if (IsSelected)
                paper.FillPolygon(new SolidBrush(Color), points);
        }

        public override void Delete()
        {
            base.Delete();
            Relationships.Remove(this);
        }
    }
}
