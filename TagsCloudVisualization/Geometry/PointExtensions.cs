using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
    public static class PointExtensions
    {
        public static Vector ToVector(this Point point)
        {
            return new Vector(point);
        }

        public static double DistanceTo(this Point from, Point to)
        {
            return (new Vector(from) - new Vector(to)).Length;
        }
    }
}