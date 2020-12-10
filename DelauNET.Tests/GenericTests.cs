using System;
using System.Collections.Generic;
using System.Linq;
using DelauNET.Model;
using DelauNET.Triangulation;
using Xunit;
using Xunit.Abstractions;

namespace DelauNET.Tests
{
    public class GenericTests
    {
        private ITestOutputHelper _testOutputHelper;
        public GenericTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        private static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new Triangle(new Vertex(0, 0), new Vertex(0, 1), new Vertex(1, 0)),
                new List<Vertex>{new Vertex(0,0), new Vertex(0, 1), new Vertex(1, 0)}
            };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void TriangulationTest(Triangle expected, List<Vertex> vertices)
        {
            BowyerWatsonTriangulator triangulator = new BowyerWatsonTriangulator();
            triangulator.Initialize(vertices);
            triangulator.ProcessAll();

            _testOutputHelper.WriteLine("Expected");
            _testOutputHelper.WriteLine(expected.ToString());

            _testOutputHelper.WriteLine("Actual");
            _testOutputHelper.WriteLine(triangulator.Triangles[0].ToString());

            _testOutputHelper.WriteLine("Comparison");
            _testOutputHelper.WriteLine((expected == triangulator.Triangles[0]).ToString());

            Assert.True(triangulator.Triangles.SequenceEqual(new[]{expected}));
        }
    }
}