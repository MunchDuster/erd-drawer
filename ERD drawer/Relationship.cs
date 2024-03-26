using System.Text.Json.Serialization;

namespace ERD_drawer
{
    public class Relationship : AttributedDisplayable
    {
        public class RelationshipLink
        {
            public RelationshipLink(int entityId, string quantity)
            {
                EntityId = entityId;
                Quantity = quantity;
            }
            public int EntityId { get; set; }
            public string Quantity { get; set; }

            [JsonIgnore] public Entity Entity => Entity.Entities.Find(entity => entity.Id == EntityId)!;
        }
        
        [JsonIgnore] const int numberPaddingX = -10;
        [JsonIgnore] const int numberPaddingY = 10;

        [JsonIgnore] public static readonly Color color = Color.Blue;
        [JsonIgnore] protected override Color Color => color;

        [JsonIgnore] public static List<Relationship> Relationships = new();

        [JsonIgnore] public static Dictionary<string, List<Relationship>> RelationshipsMap = new();

        [JsonIgnore] protected override int TextLeft => 15;
        [JsonIgnore] protected override int TextTop => 28;
        [JsonIgnore] public override int Height => 80;
        public bool IsUnary { get; set; }

        public List<RelationshipLink> Links;
        public double lineBias { get; set; }

        [JsonIgnore] private string key => string.Join("-", Links);

        [JsonIgnore] private List<Point> startPoints => new()
        { // Anti clockwise
            new(Right  , MiddleY),
            new(MiddleX, Top    ),
            new(Left   , MiddleY),
            new(MiddleX, Bottom )
        };

        [JsonIgnore]
        private List<Func<Displayable, int>> Orderers => new()
        { // Anti clockwise
            (Displayable displayable) => displayable.MiddleX - MiddleX,
            (Displayable displayable) => -(displayable.MiddleY - MiddleY),
            (Displayable displayable) => -(displayable.MiddleX - MiddleX),
            (Displayable displayable) => displayable.MiddleY - MiddleY
        };

        private void DrawLinkText(int linkIndex, int order)
        {
            string text = Links[linkIndex].Quantity;
            int right = startPoints[order].X;
            int left = startPoints[order].X - GetStringWidth(text);
            int top = startPoints[order].Y;
            int bottom = startPoints[order].Y - 20;

            Point pos = new();
            switch (order)
            {
                case 0: 
                    pos = new(right, top); 
                    break;
                case 1: 
                    pos = new(right, bottom);
                    break;
                case 2: 
                    pos = new(left, top);
                    break;
                case 3:
                    pos = new(right, top);
                    break;
            }
            paper.DrawString(text, font, brush, pos);
        }

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
                for (int i = Relationships[0].Links.Count - 1; i >= 0 ; i--)
                    if (Relationships[0].Links.Any(link => link.EntityId == entity.Id))
                        Relationships[0].Links.RemoveAt(i);
            }
            PopulateMap();
        }

        public Relationship(string name, List<Attribute> attributes, int X, int Y, List<RelationshipLink> links, double lineBias, bool isUnary) : base(name, attributes, X, Y)
        {
            Links = links;
            
            this.lineBias = lineBias;

            IsUnary = isUnary;

            form1.MouseWheel += ChangeBias;

            Relationships.Add(this);
            PopulateMap();
        }

        [JsonConstructor]
        public Relationship(string name, List<Attribute> attributes, int X, int Y, List<RelationshipLink> links, double lineBias, bool isUnary, int id) : base(name, attributes, X, Y, id)
        {
            Links = links;
            this.lineBias = lineBias;
            IsUnary = isUnary;

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
            DrawLines();
            DrawName();

            foreach (Attribute attribute in Attributes)
                attribute.Draw(attribute.IsSelected);
        }

        private void DrawLines()
        {
            // special case for unary relationship
            if (IsUnary)
            {
                SplitLine.DrawHorizontalSplitLine(
                    startPoints[2],
                    Math.Min(Left, Links[0].Entity.Left) - 10,
                    new(Links[0].Entity.Left, Links[0].Entity.MiddleY)
                    );
                DrawLinkText(0, 2);

                SplitLine.DrawHorizontalSplitLine(
                   startPoints[0],
                   Math.Max(Right, Links[0].Entity.Right) + 10,
                   new(Links[0].Entity.Right, Links[0].Entity.MiddleY)
                   );
                DrawLinkText(1, 0);
                return;
            }

            int[] startIndices = GetStartPointOrder();
            for (int i = 0; i < Links.Count; i++)
            {
                Entity entity = Links[i].Entity;
                Point start = startPoints[startIndices[i]];
                SplitLine.DrawHorizontalSplitLine(this, entity, start);
                DrawLinkText(i, startIndices[i]);
            }
        }

        /// <summary>
        /// Calculates the optimal startPoint for each link
        /// </summary>
        /// <returns></returns>
        private int[] GetStartPointOrder()
        {
            List<int> orderersLeft = new();
            for (int i = 0; i < Orderers.Count; i++)
            {
                orderersLeft.Add(i);
            }

            int[] order = new int[Links.Count];

            for (int i = 0; i < Links.Count; i++)
            {
                int index = -1;
                int highestScore = int.MinValue;
                for(int j = 0; j < orderersLeft.Count; j++)
                {
                    int score = Orderers[orderersLeft[j]](Links[i].Entity);
                    if (score > highestScore)
                    {
                        index = orderersLeft[j];
                        highestScore = score;
                    }
                }
                order[i] = index;
                orderersLeft.Remove(index);
            }
            return order;
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
