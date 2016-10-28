using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualisationTests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void PutPointRectangle_InCenter()
        {
            var result = layouter.PutNextRectangle(new Size(0, 0));
            result.Should().Be(new Rectangle(center, new Size(0, 0)));
        }

        [Test]
        public void PutSingleRectangle_Centered()
        {
            var result = layouter.PutNextRectangle(new Size(10, 10));
            result.Should().Be(new Rectangle(new Point(-5, -5), new Size(10, 10)));
        }

        [Test]
        public void PutRectangles_WithNoIntersections()
        {
            var rectangles = PutRectangles(new Size(5, 10), new Size(5, 15), new Size(10, 5));
            AssertNoIntersections(rectangles);
        }

        [TestCase(20, 3)]
        [TestCase(100, 10)]
        [TestCase(200, 25)]
        public void PutRectangles_InCircularCloud(int totalCount, int testingCount)
        {
            var sizes = Enumerable.Repeat(new Size(5, 10), totalCount).ToArray();
            var rectangles = PutRectangles(sizes);
            var lastRectangles = rectangles.Skip(totalCount - testingCount).ToArray();

            var sampleRadius = lastRectangles.Last().MaxDistance(center);
            var dRadius = lastRectangles.Max(rect => Math.Abs(sampleRadius - rect.MaxDistance(center)));
            (dRadius / sampleRadius * 100).Should().BeLessThan(10);
        }

        
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        public void PutRectangles_InDenseCloud(int count)
        {
            var sizes = Enumerable.Repeat(new Size(5, 10), count).ToArray();
            var rectangles = PutRectangles(sizes);

            var circleRadius = rectangles.Select(rect => rect.MaxDistance(center)).Max();
            var circleArea = Math.PI * circleRadius * circleRadius;
            var rectanglesArea = sizes.Sum(s => s.Height * s.Width);
            (rectanglesArea / circleArea * 100).Should().BeGreaterOrEqualTo(60);
        }

        [Test]
        public void PutRectangles_InNonDefaultCenter()
        {
            center = new Point(50, 70);
            layouter = new CircularCloudLayouter(center);
            var sizes = new[] {new Size(5, 10), new Size(15, 5), new Size(1, 3)};
            var rectangles = PutRectangles(sizes);

            var distance = rectangles.Max(rect => rect.GetCenter().DistanceTo(center));
            var maxPossibleDistance = sizes.Select(size => size.Width + size.Height).Sum();
            distance.Should().BeLessOrEqualTo(maxPossibleDistance);
        }


        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var filename =
                    $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.FullName}.png";
                var rectangles = layouter.Rectangles.ToArray();
                var size = CloudVizualizer.CalcCloudSize(rectangles);
                var offset = new Size(size.Width / 2, size.Height / 2);
                var shiftedRectangles = rectangles.Select(rect => rect.Shift(offset)).ToArray();
                using (var bitmap = CloudVizualizer.DrawRectangles(size, shiftedRectangles, Color.Red))
                    bitmap.Save(filename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }

        private void AssertNoIntersections(List<Rectangle> rectangles)
        {
            for (int i = 0; i < rectangles.Count - 1; i++)
            {
                for (int j = i + 1; j < rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        Assert.Fail("Rectangles {0} and {1} are intersected", rectangles[i], rectangles[j]);
                }
            }
        }

        private List<Rectangle> PutRectangles(params Size[] rectangleSizes)
        {
            return rectangleSizes.Select(size => layouter.PutNextRectangle(size)).ToList();
        }

    }
}
