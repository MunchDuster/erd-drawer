namespace ERD_drawer
{
    public static class SplitLine
    {
        public static Graphics paper;
        public static Pen pen;

        /// <summary>
        /// Lines connect from starting point on A specified to side of B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void DrawHorizontalSplitLine(Displayable anchor, Displayable connected, Point start)
        {
            bool isAnchorOnRight = anchor.Right > connected.Right;
            Displayable right = isAnchorOnRight ? anchor : connected;
            Displayable left = isAnchorOnRight ? connected : anchor;
            Point leftRight = new(left.Right, left.MiddleY);
            Point rightLeft = new(right.Left, right.MiddleY);
            int midX = Lerp(left.Right, right.Left, 0.5);

            bool isOverTop = right.Left <= left.Right;

            if (isOverTop)
            {
                bool closerToLeft = Math.Abs(left.Left - right.Left) < Math.Abs(left.Right - right.Right);

                if (closerToLeft)
                {
                    rightLeft.X = right.Left;
                    leftRight.X = left.Left;
                    midX = Math.Min(left.Left, right.Left) - 10;
                }
                else
                {
                    rightLeft.X = right.Right;
                    leftRight.X = left.Right;
                    midX = Math.Max(left.Right, right.Right) + 10;
                }
            }

            Point end = isAnchorOnRight ? leftRight : rightLeft;
            DrawHorizontalSplitLine(start, midX, end);
        }

        public static void DrawHorizontalSplitLine(Point start, int midX, Point end)
        {
            Point mid1 = new(midX, start.Y);
            Point mid2 = new(midX, end.Y);
            DrawSplitLine(start, mid1, mid2, end);
        }

        /// <summary>
        /// Draws line connecting two displayables that would be diagonal in three parts such that there are no diagonal lines 
        /// Lines connect to top/bottom of displayables (horizontal)
        /// </summary>
        public static void DrawVerticalSplitLine(Displayable anchor, Displayable connected)
        {
            bool isAOnTop = anchor.Top < connected.Top;
            Displayable top = isAOnTop ? anchor : connected;
            Displayable bottom = isAOnTop ? connected : anchor;

            bool isBySide = top.Bottom >= bottom.Top - 10;

            Point topBottom = new(top.MiddleX, top.Bottom);
            Point bottomTop = new(bottom.MiddleX, bottom.Top);
            int midY = Lerp(topBottom.Y, bottomTop.Y, 0.5);

            if (isBySide)
            {
                bool closerToBottom = connected.MiddleY >= anchor.MiddleY;

                if (closerToBottom)
                {
                    bottomTop.Y = bottom.Bottom;
                    topBottom.Y = top.Bottom;
                    midY = Math.Max(bottom.Bottom, top.Bottom) + 10;
                }
                else
                {
                    bottomTop.Y = bottom.Top;
                    topBottom.Y = top.Top;
                    midY = Math.Min(bottom.Top, top.Top) - 10;
                }
            }

            

            DrawSplitLine(topBottom, new(topBottom.X, midY), new(bottomTop.X, midY), bottomTop);
        }

        public static void DrawVerticalCornerLine(Displayable anchor, Displayable connected)
        {
            bool isAnchorOnTop = anchor.Top < connected.Top;
            Point start = new(anchor.MiddleX, isAnchorOnTop ? anchor.Bottom : anchor.Top);

            bool isConnectOnLeft = connected.Left < anchor.Left;
            Point end = new(isConnectOnLeft ? connected.Right : connected.Left, connected.MiddleY);

            Point mid = new(start.X, end.Y);
            paper.DrawLine(pen, start, mid);
            paper.DrawLine(pen, mid, end);
        }

        public static void DrawSplitLine(Point start, Point mid1, Point mid2, Point end)
        {
            paper.DrawLine(pen, start, mid1);
            paper.DrawLine(pen, mid1, mid2);
            paper.DrawLine(pen, mid2, end);
        }

        public static int Lerp(int from, int to, double amount)
        {
            return from + (int)((to - from) * amount);
        }
    }
}
