using System;
using System.Text;

namespace DelauNET.Model
{
    public struct Vertex : IComparable<Vertex>, IEquatable<Vertex>
    {
        public readonly float X, Y;

        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float DistanceTo(Vertex other)
        {
            float xDiff = Math.Abs(X - other.X);
            float yDiff = Math.Abs(Y - other.Y);
            return (float) Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }

        public bool InRadius(Circle circle) => DistanceTo(circle.Origin) < circle.Radius;


        public static Vertex operator +(Vertex lhs, Vertex rhs)
        {
            return new Vertex(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Vertex operator -(Vertex lhs, Vertex rhs)
        {
            return new Vertex(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Vertex operator /(Vertex lhs, float rhs)
        {
            return new Vertex(lhs.X / rhs, lhs.Y / rhs);
        }

        public int CompareTo(Vertex other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }

        public bool Equals(Vertex other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            return obj is Vertex vertex && Equals(vertex);
        }

        public static bool operator ==(Vertex lhs, Vertex rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vertex lhs, Vertex rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }


        public override string ToString() => $"Vertex: ({X}, {Y})";
    }
}