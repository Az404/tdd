using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double AngleStep = Math.PI/32;
        private const double CompactionTolerance = 1;

        private Point center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var candidates = new List<Rectangle>();
            for (double angle = 0; angle < 2*Math.PI; angle += AngleStep)
            {
                var rectCenter = GetPoint(GetPossibleRadius(rectangleSize), angle);
                var rect = CreateRectangle(rectCenter, rectangleSize);
                candidates.Add(PushToCloud(rect));
            }
            var result = candidates.OrderBy(rect => Geometry.Distance(rect.GetCenter(), center)).First();
            rectangles.Add(result);
            return result;
        }

        private double GetPossibleRadius(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return 0;
            return rectangles.Max(rect => rect.MaxDistance(center)) + Math.Max(rectangleSize.Height, rectangleSize.Width);
        }

        private Point GetPoint(double radius, double angle)
        {
            return new Point(
                (int)(radius * Math.Cos(angle)) + center.X,
                (int)(radius * Math.Sin(angle)) + center.Y
            );
        }
        
        private bool HasIntersections(Rectangle target)
        {
            return rectangles.Any(target.IntersectsWith);
        }

        private Rectangle CreateRectangle(Point rectCenter, Size rectSize)
        {
            return new Rectangle(rectCenter - new Size(rectSize.Width / 2, rectSize.Height / 2), rectSize);
        }

        private Rectangle PushToCloud(Rectangle target)
        {
            var centerVector = center.ToVector();
            var maxRadiusVector = (target.GetCenter().ToVector() - centerVector);
            var minRadiusVector = new Vector(0, 0);
            while ((maxRadiusVector - minRadiusVector).Length > CompactionTolerance)
            {
                var radiusVector = (maxRadiusVector + minRadiusVector) * 0.5;
                var rect = CreateRectangle((centerVector + radiusVector).ToDrawingPoint(), target.Size);

                if (HasIntersections(rect))
                    minRadiusVector = radiusVector;
                else
                    maxRadiusVector = radiusVector;
            }
            return CreateRectangle((centerVector + maxRadiusVector).ToDrawingPoint(), target.Size);
        }
    }
}
