using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class Geometry
    {
        public static double Distance(Point a, Point b)
        {
            return (new Vector(a) - new Vector(b)).Length;
        }
    }
    
    public class Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point p) : this(p.X, p.Y) {}

        public readonly double X;
        public readonly double Y;

        public double Length => Math.Sqrt(X * X + Y * Y);

        public double Angle => Math.Atan2(Y, X);

        public static Vector Zero = new Vector(0, 0);

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
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, double k)
        {
            return new Vector(a.X * k, a.Y * k);
        }

        public static Vector operator *(double k, Vector a)
        {
            return a * k;
        }

        public static double operator *(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static double operator ^(Vector a, Vector b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public Vector Normalize()
        {
            return Length > 0 ? this * (1 / Length) : this;
        }

        public Vector Rotate(double angle)
        {
            return new Vector(X * Math.Cos(angle) - Y * Math.Sin(angle), X * Math.Sin(angle) + Y * Math.Cos(angle));
        }

        public Point ToDrawingPoint()
        {
            return new Point((int)X, (int)Y);
        }

        public PointF ToDrawingPointF()
        {
            return new PointF((float) X, (float) Y);
        }

        public double GetAngle(Vector vector)
        {
            return Math.Abs(this.Angle - vector.Angle);
        }
    }
}