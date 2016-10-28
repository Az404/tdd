﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Geometry
{
    public static class RectangleExtensions
    {

        public static double MaxDistance(this Rectangle rect, Point point)
        {
            return new[] {
                rect.Location.DistanceTo(point),
                new Point(rect.X, rect.Bottom).DistanceTo(point),
                new Point(rect.Right, rect.Y).DistanceTo(point),
                (rect.Location + rect.Size).DistanceTo(point)
            }.Max();
        }

        public static Point GetCenter(this Rectangle rect)
        {
            return rect.Location + new Size(rect.Width / 2, rect.Height / 2);
        }

        public static bool HasIntersectionsWith(this Rectangle target, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(target.IntersectsWith);
        }

        public static Rectangle Shift(this Rectangle rectangle, Size offset)
        {
            return new Rectangle(rectangle.Location + offset, rectangle.Size);
        }
    }
}
