﻿using System;
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

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Point center;
        private List<Rectangle> rectangles;

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

        [Test]
        public void PutRectangles_WithNoIntersections()
        {
            rectangles = PutRectangles(new Size(5, 10), new Size(5, 15), new Size(10, 5));
            AssertNoIntersections(rectangles);
        }

        [TestCase(20, 3)]
        [TestCase(100, 10)]
        [TestCase(200, 25)]
        public void PutRectangles_InCircularCloud(int totalCount, int testingCount)
        {
            var sizes = Enumerable.Repeat(new Size(5, 10), totalCount).ToArray();
            rectangles = PutRectangles(sizes);
            var lastRectangles = rectangles.Skip(totalCount - testingCount).ToArray();

            var sampleRadius = lastRectangles.Last().MaxDistance(center);
            var dRadius = lastRectangles.Max(rect => Math.Abs(sampleRadius - rect.MaxDistance(center)));
            (dRadius / sampleRadius * 100).Should().BeLessThan(10);
        }


        [Test]
        public void PutRectangles_InDenseCloud()
        {
            var sizes = Enumerable.Repeat(new Size(5, 10), 100).ToArray();
            rectangles = PutRectangles(sizes);

            var circleRadius = rectangles.Select(rect => rect.MaxDistance(center)).Max();
            var circleArea = Math.PI * circleRadius * circleRadius;
            var rectanglesArea = sizes.Sum(s => s.Height * s.Width);
            (rectanglesArea / circleArea * 100).Should().BeGreaterOrEqualTo(70);
        }


        [TearDown]
        public void TearDown()
        {
            if (Equals(TestContext.CurrentContext.Result.Outcome, ResultState.Failure))
            {
                const int imageSize = 1024;
                var bitmap = new Bitmap(imageSize, imageSize);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    foreach (var rect in rectangles)
                    {
                        var shiftedRect = new Rectangle(rect.Location + new Size(imageSize / 2, imageSize / 2), rect.Size);
                        graphics.DrawRectangle(Pens.Red, shiftedRect);
                    }
                }
                var filename =
                    $"{TestContext.CurrentContext.TestDirectory}\\{TestContext.CurrentContext.Test.FullName}.bmp";
                bitmap.Save(filename);
                TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }

    }
}
