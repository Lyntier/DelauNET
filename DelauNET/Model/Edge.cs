using System;
using System.Linq;
using System.Reflection;

namespace DelauNET.Model
{
    public struct Edge : IComparable<Edge>, IEquatable<Edge>
    {
        public readonly Vertex P1, P2;

        private readonly bool _directionalEdge;

        public Edge(Vertex p1, Vertex p2, bool directionalEdge = false)
        {
            P1 = p1;
            P2 = p2;
            _directionalEdge = directionalEdge;
        }

        public override bool Equals(object other)
        {
            return other is Edge edge && Equals(edge);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                if (_directionalEdge) // Order makes different edges.
                {
                    hashCode = P1.GetHashCode();
                    hashCode = (hashCode * 397) ^ P2.GetHashCode();
                }
                else // Order doesn't make different edges.
                {
                    var sorted = new[] {P1, P2}.OrderBy(vertex => vertex).ToArray();
                    hashCode = sorted[0].GetHashCode();
                    hashCode = hashCode * 397 ^ sorted[1].GetHashCode();
                }
                return hashCode;
            }
        }

        public int CompareTo(Edge other)
        {
            var edge1Sorted = new[] {P1, P2}.OrderBy(vertex => vertex).ToArray();
            var edge2Sorted = new[] {other.P1, other.P2}.OrderBy(vertex => vertex).ToArray();

            var p1Comparison = edge1Sorted[0].CompareTo(edge2Sorted[0]);
            // If first two vertices are equal, return comparison of last two vertices.
            // Fine with both directional edges and positional edges.
            // TODO: Should directional edges that are equivalent to their positional counterparts be considered equal?
            return p1Comparison != 0 ? p1Comparison : edge1Sorted[1].CompareTo(edge2Sorted[1]);
        }

        public bool Equals(Edge other)
        {
            return CompareTo(other) == 0;
        }


        public static bool operator ==(Edge lhs, Edge rhs)
        {
            return lhs.CompareTo(rhs) == 0;
        }

        public static bool operator !=(Edge lhs, Edge rhs)
        {
            return !(lhs == rhs);
        }


        public Vertex Center
        {
            get
            {
                var centerX = (P1.X + P2.X) / 2f;
                var centerY = (P1.Y + P2.Y) / 2f;
                return new Vertex(centerX, centerY);
            }
        }

        public Edge Bisector
        {
            get
            {
                if (Math.Abs(P1.X - P2.X) < float.Epsilon)
                {
                    return new Edge(Center, new Vertex(Center.X + 1, Center.Y));
                }

                if (Math.Abs(P1.Y - P2.Y) < float.Epsilon)
                {
                    return new Edge(Center, new Vertex(Center.X, Center.Y + 1));
                }

                float slope = (P2.Y - P1.Y) / (P2.X - P1.X);
                slope = 0f - 1f / slope;
                return new Edge(Center, new Vertex(Center.X + 1, Center.Y + slope));
            }
        }

        public static Vertex GetIntersection(Edge ab, Edge bc)
        {
            // P1 stores their starting position, and P2 their direction.
            // We use these to convert the bisectors into line segments.
            var s1 = ab.P1;
            var e1 = ab.P2;

            var s2 = bc.P1;
            var e2 = bc.P2;

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

        public override string ToString()
        {
            return $"Edge: {P1.ToString()} ==> {P2.ToString()}";
        }
    }
}