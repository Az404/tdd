using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualisationTests
{
    [TestFixture]
    class CloudVisualizer_Should
    {
        [Test]
        public void CalculateOrdinaryCloudSize_Correctly()
        {
            CloudVizualizer.CalcCloudSize(new[]
            {
                new Rectangle(0, 0, 5, 25), new Rectangle(-5, -10, 15, 20)
            }).Should().Be(new Size(15, 35));
        }

        [Test]
        public void CalculateCloudSize_Correctly_WhenEmptyCollection()
        {
            CloudVizualizer.CalcCloudSize(new Rectangle[]{}).Should().Be(new Size(0, 0));
        }
    }
}
