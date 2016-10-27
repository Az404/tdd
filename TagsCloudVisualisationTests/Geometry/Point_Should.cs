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
    class Point_Should
    {
        [Test]
        public void CalcDistanceToOtherPoint_Correctly()
        {
            new Point(1, 2).DistanceTo(new Point(5, 5)).Should().Be(5);
        }
    }
}
