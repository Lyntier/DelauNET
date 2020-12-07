using System;
using System.Collections.Generic;
using System.Linq;
using DelauNET.Model;
using DelauNET.Triangulation;
using Xunit;
using Xunit.Abstractions;

namespace DelauNET.Tests
{
    public class TriangulationTests
    {

        public static IEnumerable<object[]> SuperTrianglePoints()
        {
            yield return new object[]
            {
                new List<Vertex>
                {
                    new Vertex(0, 0),
                    new Vertex(0, 1),
                    new Vertex(1, 1),
                    new Vertex(-1, -2)
                },
                new Triangle(new Vertex(-5, -5), new Vertex(0, 4), new Vertex(5, -5), true),
            };
        }

        public static IEnumerable<object[]> GoodSmallTriangulation1()
        {
            yield return new object[]
            {
                new List<Vertex>
                {
                    new Vertex(0, 0),
                    new Vertex(1, 2),
                    new Vertex(2, 1)
                },
                new List<Triangle>
                {
                    new Triangle(new Vertex(0, 0), new Vertex(1, 2), new Vertex(2, 1))
                }
            };
        }

        public static IEnumerable<object[]> GoodSmallTriangulation2()
        {
            yield return new object[]
            {
                new List<Vertex>
                {
                    new Vertex(-2, 0),
                    new Vertex(-1, -1),
                    new Vertex(0, 1),
                    new Vertex(0, -1),
                    new Vertex(0, -1)
                },
                new List<Triangle>
                {
                    new Triangle(new Vertex(-2, 0), new Vertex(-1, -1), new Vertex(0, 1)),
                    new Triangle(new Vertex(-1, -1), new Vertex(0, -1), new Vertex(0, 1))
                }
            };
        }


        [Theory]
        [MemberData(nameof(SuperTrianglePoints))]
        public void SuperTriangles(List<Vertex> vertices, Triangle expected)
        {
            var supertriangle = BowyerWatsonTriangulator.CreateSuperTriangle(vertices);
            Assert.Equal(expected, supertriangle);
        }

        [Theory]
        [MemberData(nameof(GoodSmallTriangulation1))]
        public void SmallTriangulation1(List<Vertex> vertices, List<Triangle> expected)
        {
            ITriangulator triangulator = new BowyerWatsonTriangulator();
            List<Triangle> triangles = triangulator.Triangulate(vertices).ToList();
            Assert.True(expected.OrderBy(n => n)
                    .SequenceEqual(triangles.OrderBy(n => n)),
                "Can triangulate 3 points into a triangle");
        }

        [Theory]
        [MemberData(nameof(GoodSmallTriangulation2))]
        public void SmallTriangulation2(List<Vertex> vertices, List<Triangle> expected)
        {
            ITriangulator triangulator = new BowyerWatsonTriangulator();
            List<Triangle> triangles = triangulator.Triangulate(vertices).ToList();
            Assert.True(expected.OrderBy(n => n)
                    .SequenceEqual(triangles.OrderBy(n => n)),
                "Can triangulate 3 points into a triangle");
        }
    }
}