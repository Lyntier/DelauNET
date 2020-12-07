using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelauNET.Model
{
    public struct Triangle : IComparable<Triangle>, IEquatable<Triangle>
    {

        public readonly Vertex A, B, C;
        public readonly Edge AB, BC, CA;

        public bool SuperTri;

        public Triangle(Vertex a, Vertex b, Vertex c, bool superTriangle = false)
        {
            if (a == b || b == c || a == c) throw new ArgumentException("Can't create a triangle from equal points");

            A = a;
            B = b;
            C = c;

            AB = new Edge(A, B);
            BC = new Edge(B, C);
            CA = new Edge(C, A);

            SuperTri = superTriangle;
        }

        public bool HasVertex(Vertex vertex) => A == vertex || B == vertex || C == vertex;

        public Vertex Circumcenter => GetCircumcenter(this);

        public static Vertex GetCircumcenter(Triangle triangle)
        {
            // Circumcenter of a triangle is the intersection of perpendicular bisectors.
            // This is the point at which you can draw a circle going through all three
            // points of the triangle.

            // First we get the bisectors (Helper method in Edge class)
            var edge1 = triangle.AB.Bisector;
            var edge2 = triangle.BC.Bisector;

            // P1 stores their starting position, and P2 their direction.
            // We use these to convert the bisectors into line segments.
            var s1 = edge1.P1;
            var e1 = edge1.P1 + edge1.P2;

            var s2 = edge2.P1;
            var e2 = edge2.P1 + edge2.P2;

            // You can calculate the intersection of lines when you have the lines as
            // formulas ( ax + by = c ), where:
            float a1 = e1.Y - s1.Y;
            float b1 = s1.X - e1.X;
            float c1 = a1 * s1.X + b1 * s1.Y;

            float a2 = e2.Y - s2.Y;
            float b2 = s2.X - e2.X;
            float c2 = a2 * s2.X + b2 * s2.Y;

            // Difference between points.
            float delta = a1 * b2 - a2 * b1;

            // If that difference is 0, they'll never meet.
            if (Math.Abs(delta) < float.Epsilon) throw new ArgumentException("Parallel!");

            // Calculate the point where the two lines would meet.
            float x = (b2 * c1 - b1 * c2) / delta;
            float y = (a1 * c2 - a2 * c1) / delta;

            return new Vertex(x, y);
        }

        public Circle Circumcircle => GetCircumcircle(this);

        static Circle GetCircumcircle(Triangle triangle)
        {
            Vertex circumcenter = triangle.Circumcenter;
            return new Circle(circumcenter, circumcenter.DistanceTo(triangle.A));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            var superTriStr = SuperTri ? " (Super triangle)" : string.Empty;

            sb.AppendLine($"=== Triangle ==={superTriStr}");
            sb.AppendLine($"A: {A.ToString()} || B: {B.ToString()} || C: {C.ToString()}");

            return sb.ToString();
        }


        public int CompareTo(Triangle other)
        {
            // Using a sorted set for this;
            SortedSet<Vertex> lhs = new SortedSet<Vertex> {A, B, C};
            SortedSet<Vertex> rhs = new SortedSet<Vertex> {other.A, other.B, other.C};

            if (lhs.Count != 3) throw new ArgumentException($"Illegal triangle: {lhs}");
            if (rhs.Count != 3) throw new ArgumentException($"Illegal triangle: {rhs}");

            for (int i = 0; i < lhs.Count; i++)
            {
                var lVert = lhs.ElementAt(i);
                var rVert = rhs.ElementAt(i);

                // If there's an inequality between the two vertices, return that inequality.
                // (mostly used for triangle equality checks, but triangles can also be sorted this way)
                int comparison;
                if ((comparison = lVert.CompareTo(rVert)) != 0) return comparison;
            }

            // At this point all vertices compare equally.
            return 0;
        }

        public bool Equals(Triangle other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            return obj is Triangle triangle && Equals(triangle);
        }

        public static bool operator ==(Triangle lhs, Triangle rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Triangle lhs, Triangle rhs)
        {
            return !(lhs == rhs);
        }
        
        public override int GetHashCode()
        {
            SortedSet<Vertex> vertices = new SortedSet<Vertex> {A, B, C};
            unchecked
            {
                var hashCode = vertices.ElementAt(0).GetHashCode();
                hashCode = (hashCode * 397) ^ vertices.ElementAt(1).GetHashCode();
                hashCode = (hashCode * 397) ^ vertices.ElementAt(2).GetHashCode();
                return hashCode;
            }
        }
    }
}