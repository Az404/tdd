using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{

    public static class PointExtensions
    {
        public static Vector ToVector(this Point point)
        {
            return new Vector(point);
        }
    }

    public static class RectangleExtensions
    {

        public static double MaxDistance(this Rectangle rect, Point point)
        {
            return new[] {
                Geometry.Distance(rect.Location, point),
                Geometry.Distance(new Point(rect.X, rect.Bottom), point),
                Geometry.Distance(new Point(rect.Right, rect.Y), point),
                Geometry.Distance(rect.Location + rect.Size, point)
            }.Max();
        }

        public static Point GetCenter(this Rectangle rect)
        {
            return rect.Location + new Size((int)(rect.Width / 2), (int)(rect.Height / 2));
        }
    }
}
