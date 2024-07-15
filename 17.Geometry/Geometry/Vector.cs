using GeometryTasks;
using System.Numerics;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }
}

namespace ReadOnlyVector
{
    public class ReadOnlyVector
    {
        public readonly double X;
        public readonly double Y;

        public ReadOnlyVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ReadOnlyVector Add(ReadOnlyVector other)
        {
            return new ReadOnlyVector(X + other.X, Y + other.Y);
        }

        public ReadOnlyVector WithX(double y)
        {
            return new ReadOnlyVector(X, y);
        }

        public ReadOnlyVector WithY(double x)
        {
            return new ReadOnlyVector(x, Y);
        }
    }
}