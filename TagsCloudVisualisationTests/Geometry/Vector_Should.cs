using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualisationTests.Geometry
{
    [TestFixture]
    public class Vector_Should
    {
        [TestCase(3, 4, ExpectedResult = 5, Description = "when ordinary vector")]
        [TestCase(0, 4, ExpectedResult = 4, Description = "when x is zero")]
        [TestCase(3, 0, ExpectedResult = 3, Description = "when y is zero")]
        [TestCase(0, 0, ExpectedResult = 0, Description = "when zero-vector")]
        public double HasCorrectLength(double x, double y)
        {
            var vector = new Vector(x, y);
            return vector.Length;
        }

        private double[] UnpackVector(Vector v)
        {
            return new[] { v.X, v.Y };
        }

        [TestCase(2, 5, 9, 8, ExpectedResult = new[] {11, 13}, Description = "when ordinary vectors")]
        [TestCase(2, 5, 0, 0, ExpectedResult = new[] { 2, 5 }, Description = "with zero vectors")]
        [TestCase(-2, 5, 9, -8, ExpectedResult = new[] { 7, -3 }, Description = "with negative coordinates in vectors")]
        public double[] ReturnCorrectSum(int x1, int y1, int x2, int y2)
        {
            return UnpackVector(new Vector(x1, y1) + new Vector(x2, y2));
        }

        [TestCase(20, 50, 9, 8, ExpectedResult = new[] { 11, 42 }, Description = "when ordinary vectors")]
        [TestCase(2, 5, 0, 0, ExpectedResult = new[] { 2, 5 }, Description = "with zero vector")]
        [TestCase(0, 0, 7, 3, ExpectedResult = new[] { -7, -3 }, Description = "when minuend is zero")]
        [TestCase(2, 5, -5, 8, ExpectedResult = new[] { 7, -3 }, Description = "when subtrahend has negative coordinate")]
        public double[] ReturnCorrectDifference(int x1, int y1, int x2, int y2)
        {
            return UnpackVector(new Vector(x1, y1) - new Vector(x2, y2));
        }

        [TestCase(3, 7, 2, ExpectedResult = new[] { 6, 14 }, Description = "when scalar is integer")]
        [TestCase(2, 5, 0, ExpectedResult = new[] { 0, 0 }, Description = "when scalar is zero")]
        [TestCase(2, 5, 0.5, ExpectedResult = new[] { 1, 2.5 }, Description = "when scalar is real")]
        public double[] ReturnCorrectProductWithScalar(int x1, int y1, double scalar)
        {
            return UnpackVector(new Vector(x1, y1) * scalar);
        }

        [Test]
        public void CorrectlyCreates_FromPolarCoordinates()
        {
            var eps = 1e-9;
            var vector = Vector.FromPolar(2, Math.PI / 6);
            vector.X.Should().BeApproximately(Math.Sqrt(3), eps);
            vector.Y.Should().BeApproximately(1, eps);
        }
    }
}
