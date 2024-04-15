using System.Text.Json.Serialization;

namespace ERD_drawer
{
    public static class Settings
    {
        // Font settings
        public static int fontSize = 10;
        public static string font = "Consolas";

        public static double fontWidthPerCharMultiplier = 1.3;
        public static double fontHeightPerCharMultiplier = 2.4;

        [JsonIgnore] public static int fontWidthPerChar => (int)(fontSize * fontWidthPerCharMultiplier);
        [JsonIgnore] public static int fontHeightPerChar => (int)(fontSize * fontHeightPerCharMultiplier);

        // Displayable settings
        public static int height;
        public static int padding;

        public static void Apply()
        {
            Displayable.font = new(familyName: font, fontSize);
            SplitLine.pen = new(Color.Black);
        }
    }
}
