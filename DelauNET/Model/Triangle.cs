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
        public readonly Circle Circumcircle;

        public bool SuperTri;

        public Triangle(Edge edge, Vertex vertex, bool superTriangle = false) : this(edge.P1, edge.P2, vertex, superTriangle)
        {
        }
        
        public Triangle(Vertex a, Vertex b, Vertex c, bool superTriangle = false)
        {
            if (a == b || b == c || a == c) throw new ArgumentException("Can't create a triangle from equal points");

            A = a;
            B = b;
            C = c;

            AB = new Edge(A, B);
            BC = new Edge(B, C);
            CA = new Edge(C, A);

            Circumcircle = GetCircumcircle(AB, BC);


            SuperTri = superTriangle;
        }

        // Checks if the given vertex is part of this triangle.
        public bool HasVertex(Vertex vertex) => A == vertex || B == vertex || C == vertex;

        public bool HasEdge(Edge edge) => AB == edge || BC == edge || CA == edge;


        private static Vertex GetIntersection(Edge ab, Edge bc)
        {
            // Circumcenter of a triangle is the intersection of perpendicular bisectors.
            // This is the point at which you can draw a circle going through all three
            // points of the triangle.

            // First we get the bisectors (Helper method in Edge class)
            var edge1 = ab.Bisector;
            var edge2 = bc.Bisector;

            return Edge.GetIntersection(edge1, edge2);
        }

        private static Circle GetCircumcircle(Edge ab, Edge bc)
        {
            Vertex circumcenter = GetIntersection(ab, bc);
            return new Circle(circumcenter, circumcenter.DistanceTo(ab.P1));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            var superTriStr = SuperTri ? " (Super triangle)" : string.Empty;

            sb.AppendLine($"=== Triangle ==={superTriStr}");
            sb.AppendLine($"A: {A.ToString()} || B: {B.ToString()} || C: {C.ToString()}");

            return sb.ToString();
        }

        public static Triangle GetSupertriangle(in IList<Vertex> vertices)
        {
            IList<float> y = vertices.Select(vertex => vertex.Y).OrderBy(self => self).ToList();

            // Vertices are already sorted by X, so no need to find smallest/largest X.
            Vertex min = new Vertex(vertices[0].X, y[0]);
            Vertex max = new Vertex(vertices[vertices.Count - 1].X, y[y.Count - 1]);

            var d = max - min;
            var dMax = d.X > d.Y ? d.X : d.Y;

            Vertex mid = (max + min) / 2f;

            Vertex left = new Vertex(mid.X - 2 * dMax, mid.Y - 2 * dMax);
            Vertex top = new Vertex(mid.X, mid.Y + 2 * dMax);
            Vertex right = new Vertex(mid.X + 2 * dMax, mid.Y - 2 * dMax);

            return new Triangle(left, top, right, true);
        }


        #region Equality checks

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
            return lhs.CompareTo(rhs) == 0;
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

        #endregion
    }
}