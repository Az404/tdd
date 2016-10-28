using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double AngleStep = Math.PI/32;
        private const double CompactionTolerance = 1;

        private Point center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public IEnumerable<Rectangle> Rectangles => rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var candidates = new List<Rectangle>();
            for (var angle = 0.0; angle < 2*Math.PI; angle += AngleStep)
            {
                var rectCenter = Vector.FromPolar(GetPossibleRadius(rectangleSize), angle) + center.ToVector();
                var rect = CreateRectangle(rectCenter.ToDrawingPoint(), rectangleSize);
                candidates.Add(PushToCloud(rect));
            }
            var result = candidates.OrderBy(rect => rect.GetCenter().DistanceTo(center)).First();
            rectangles.Add(result);
            return result;
        }

        private double GetPossibleRadius(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return 0;
            return rectangles.Max(rect => rect.MaxDistance(center)) + Math.Max(rectangleSize.Height, rectangleSize.Width);
        }

        private static Rectangle CreateRectangle(Point rectCenter, Size rectSize)
        {
            return new Rectangle(rectCenter - new Size(rectSize.Width / 2, rectSize.Height / 2), rectSize);
        }

        private Rectangle PushToCloud(Rectangle target)
        {
            var centerVector = center.ToVector();
            var maxRadiusVector = target.GetCenter().ToVector() - centerVector;
            var minRadiusVector = Vector.Zero;
            while ((maxRadiusVector - minRadiusVector).Length > CompactionTolerance)
            {
                var radiusVector = (maxRadiusVector + minRadiusVector) * 0.5;
                var rect = CreateRectangle((centerVector + radiusVector).ToDrawingPoint(), target.Size);

                if (rect.HasIntersectionsWith(rectangles))
                    minRadiusVector = radiusVector;
                else
                    maxRadiusVector = radiusVector;
            }
            return CreateRectangle((centerVector + maxRadiusVector).ToDrawingPoint(), target.Size);
        }
    }
}
