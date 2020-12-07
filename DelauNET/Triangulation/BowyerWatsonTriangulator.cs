using System;
using System.Collections.Generic;
using System.Linq;
using DelauNET.Model;

namespace DelauNET.Triangulation
{
    public class BowyerWatsonTriangulator : ITriangulator
    {
        private List<Triangle> _triangles;
        public IList<Triangle> Triangles => _triangles;

        public IEnumerable<Triangle> Triangulate(List<Vertex> points)
        {
            var sorted = points
                .OrderBy(vert => vert.X)
                .ThenBy(vert => vert.X)
                .Distinct()
                .ToList();

            if (points.Count != sorted.Count)
            {
                Console.WriteLine("NOTE: Duplicate vertices in triangulation; duplicates are discarded.");
            }

            Triangle superTriangle = CreateSuperTriangle(sorted);

            _triangles = new List<Triangle> {superTriangle};
            var badTriangles = new HashSet<Triangle>();


            for (var i = 0; i < sorted.Count; i++)
            {
                var vertex = sorted[i];

                foreach (var triangle in _triangles)
                {
                    // Skip triangle if vertex is part of it.
                    if (triangle.HasVertex(vertex)) continue;

                    // Skip triangles that are already marked bad.
                    if (badTriangles.Contains(triangle)) continue;

                    // Skip triangle if vertex isn't in its circumcircle.
                    if (!vertex.InRadius(triangle.Circumcircle)) continue;

                    // Make new triangles with this vertex from triangle's edges
                    _triangles.Add(new Triangle(triangle.A, triangle.B, vertex));
                    _triangles.Add(new Triangle(vertex, triangle.B, triangle.C));
                    _triangles.Add(new Triangle(triangle.A, vertex, triangle.C));

                    // Mark this triangle as bad, because the current vertex was in its circumcircle.
                    badTriangles.Add(triangle);

                    // Need to check all vertices again if a bad triangle is found.
                    i = -1; // End of loop would set i to 1 if i is set to 0 here.

                    // Don't continue with other triangles; enumeration is modified.
                    break;
                }
            }

            _triangles.RemoveAll(triangle => badTriangles.Contains(triangle)
                                             || triangle.HasVertex(superTriangle.A)
                                             || triangle.HasVertex(superTriangle.B)
                                             || triangle.HasVertex(superTriangle.C)
            );

            _triangles = _triangles.Distinct().ToList();

            Console.WriteLine("GOOD TRIANGLES: ");
            foreach (var triangle in _triangles) Console.WriteLine(triangle.ToString());

            foreach (var vertex in points)
            {
                foreach (var triangle in _triangles)
                {
                    if (triangle.HasVertex(vertex)) continue;
                    if(vertex.InRadius(triangle.Circumcircle)) throw new ArgumentException("Wtf!");
                }
            }
            // Should only have good triangles left
            return _triangles;
        }

        public static Triangle CreateSuperTriangle(IEnumerable<Vertex> points)
        {
            points = points.ToList();
            IEnumerable<float> x = points.Select(vertex => vertex.X).ToList();
            IEnumerable<float> y = points.Select(vertex => vertex.Y).ToList();


            float xMin = x.Min();
            float xMax = x.Max();

            float yMin = y.Min();
            float yMax = y.Max();

            float xDiff = xMax - xMin;
            float yDiff = yMax - yMin;

            // Offsetting X to ensure super triangle contains all points.
            float triLeft = xMin - 2 * xDiff;
            float triRight = xMax + 2 * xDiff;

            float triDown = yMin - yDiff;
            float triUp = yMax + yDiff;

            float triMid = xMax - xDiff / 2f;

            Vertex left = new Vertex(triLeft, triDown);
            Vertex up = new Vertex(triMid, triUp);
            Vertex right = new Vertex(triRight, triDown);

            return new Triangle(left, up, right, true);
        }
    }
}