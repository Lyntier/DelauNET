using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using DelauNET.Model;

namespace DelauNET.Triangulation
{
    public class BowyerWatsonTriangulator : ITriangulator
    {
        public Vertex[] Vertices => _vertices.ToArray();
        private Vertex[] _vertices;

        public Triangle[] Triangles => _triangles.ToArray();
        private List<Triangle> _triangles;

        /// <summary>
        /// Tolerance for equality check of vertices.
        /// </summary>
        private float _tolerance;

        /// <summary>
        /// A triangle that contains all the points that need to be
        /// triangulated, required by the Bowyer-Watson algorithm.
        /// </summary>
        private Triangle _supertriangle;

        /// <summary>
        /// Whether or not the triangulator is initialized.
        /// </summary>
        private bool initialized = false;

        /// <summary> The index of the vertex that is currently being added to the Delaunay triangulation. </summary>
        private int vertexIndex = 0;

        public bool Initialize(ICollection<Vertex> vertices, float tolerance = 0.0001f)
        {
            if (vertices.Count < 3)
            {
                Console.Error.WriteLine("Less than 3 vertices given, cannot construct Delaunay from points.");
                return initialized = false;
            }

            if (tolerance < float.Epsilon)
            {
                Console.Error.WriteLine(
                    $"Could not initialize triangulator with the given fault margin: {tolerance}\n" +
                    "Required at least Single.Epsilon (smallest positive non-zero value)");
                return initialized = false;
            }

            if (vertices.Count != vertices.Distinct().Count())
            {
                Console.WriteLine("NOTE: Duplicate vertices in list. Only using unique values.");
            }

            _vertices = vertices
                .Distinct()
                .OrderBy(vertex => vertex.X)
                .ThenBy(vertex => vertex.Y)
                .ToArray();

            vertexIndex = -1;

            _triangles = new List<Triangle>(1);

            _tolerance = tolerance;

            _supertriangle = Triangle.GetSupertriangle(_vertices);
            _triangles.Add(_supertriangle);

            return initialized = true;
        }

        public void ProcessAll()
        {
            if (!initialized)
                throw new InvalidOperationException(
                    "Triangulator was not properly initialized, or completed a previous triangulation.\n" +
                    "If you need to initialize the triangulator first, use 'Initialize()'.");

            while (!ProcessNext())
            {
            }
        }

        /// <summary>
        /// Processes the next vertex in the list of vertices and adds it to the Delaunay triangulation.
        /// </summary>
        /// <returns>true if done, false if there's more points to triangulate.</returns>
        public bool ProcessNext()
        {
            if (!initialized)
                throw new InvalidOperationException(
                    "Triangulator was not properly initialized, or completed a previous triangulation.\n" +
                    "If you need to initialize the triangulator first, use 'Initialize()'.");
            vertexIndex++;
            Vertex vertex = _vertices[vertexIndex];

            HashSet<Triangle> badTriangles = new HashSet<Triangle>();

            // Find triangles that are invalid due to point insertion
            foreach (var triangle in _triangles)
            {
                if (vertex.InRadius(triangle.Circumcircle)) badTriangles.Add(triangle);
            }

            HashSet<Edge> polygon = new HashSet<Edge>();

            // Find boundary of polygonal hole
            foreach (var triangle in badTriangles)
            {
                foreach (var edge in new[] {triangle.AB, triangle.BC, triangle.CA})
                {
                    // Add edge to polygon outline if not shared by any other bad triangles
                    var badsNotThis = badTriangles.Where(self => triangle != self);
                    
                    if (!badsNotThis.Any(badTriangle => badTriangle.HasEdge(edge)))
                    {
                        polygon.Add(edge);
                    }
                }
            }

            // Remove bad triangles from triangulation
            foreach (var triangle in badTriangles)
            {
                _triangles.Remove(triangle);
            }

            // Retriangulate polygon hole with vertex
            foreach (var edge in polygon)
            {
                _triangles.Add(new Triangle(edge, vertex));
            }

            if (vertexIndex != _vertices.Length - 1) return false;

            Finish();
            initialized = false;
            return true;
        }

        public void Finish()
        {
            // Finally remove super triangle verts
            // Copy triangles array to prevent collection modification during for-loop.
            foreach (var triangle in _triangles.ToArray())
            {
                if (triangle.HasVertex(_supertriangle.A)
                    || triangle.HasVertex(_supertriangle.B)
                    || triangle.HasVertex(_supertriangle.C))
                {
                    _triangles.Remove(triangle);
                }
            }
        }
    }
}