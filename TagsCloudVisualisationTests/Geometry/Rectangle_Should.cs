using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualisationTests.Geometry
{
    [TestFixture]
    class Rectangle_Should
    {
        [Test]
        public void ReturnCorrectMaxDistance()
        {
            var rectangle = new Rectangle(new Point(0, 0), new Size(3, 4));
            rectangle.MaxDistance(new Point(3, 4)).Should().Be(5);
        }

        [Test]
        public void ReturnCorrectCenter()
        {
            new Rectangle(new Point(10, 10), new Size(20, 30)).GetCenter().Should().Be(new Point(20, 25));
        }

        [Test]
        public void ShiftsCorrectly()
        {
            var shiftedRect = new Rectangle(new Point(10, 10), new Size(20, 30)).Shift(new Size(5, 3));
            shiftedRect.Should().Be(new Rectangle(new Point(15, 13), new Size(20, 30)));
        }

        [Test]
        public void CreatesFromCenterPoint_Correctly()
        {
            RectangleExtensions.CreateRectangle(new Point(1, 1), new Size(2, 4)).Should().Be(new Rectangle(0, -1, 2, 4));
        }
    }

    [TestFixture]
    public class Rectangles_Should
    {
        [TestCase(1, 1, 5, 5, ExpectedResult = true, Description = "when intersected")]
        [TestCase(5, 5, 1, 2, ExpectedResult = false, Description = "when not intersected")]
        public bool ChecksIntersectionsCorrectly(int x, int y, int width, int height)
        {
            return new[]
            {
                new Rectangle(new Point(10, 10), new Size(20, 30)),
                new Rectangle(new Point(0, 0), new Size(2, 3))
            }.HasIntersections(new Rectangle(new Point(x, y), new Size(width, height)));
        }

        public void NotIntersects_WhenEmptyCollection()
        {
            new Rectangle[] {}.HasIntersections(new Rectangle(new Point(1, 2), new Size(3, 4))).Should().BeFalse();
        }
    }
}
