using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualisationTests.Geometry
{
    [TestFixture]
    class Point_Should
    {
        [Test]
        public void CalcDistanceToOtherPoint_Correctly()
        {
            new Point(1, 2).DistanceTo(new Point(5, 5)).Should().Be(5);
        }
    }
}
