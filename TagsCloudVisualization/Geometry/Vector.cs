using System;
using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
    public class Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point p) : this(p.X, p.Y)
        {
        }

        public readonly double X;
        public readonly double Y;

        public double Length => Math.Sqrt(X*X + Y*Y);

        public static readonly Vector Zero = new Vector(0, 0);

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        protected bool Equals(Vector other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, double k)
        {
            return new Vector(a.X*k, a.Y*k);
        }

        public static Vector operator *(double k, Vector a)
        {
            return a*k;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public Point ToDrawingPoint()
        {
            return new Point((int) X, (int) Y);
        }

        public static Vector FromPolar(double radius, double angle)
        {
            return new Vector(
                radius*Math.Cos(angle),
                radius*Math.Sin(angle)
            );
        }
    }
}