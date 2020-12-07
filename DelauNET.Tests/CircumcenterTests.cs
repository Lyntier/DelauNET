using System;
using System.Collections.Generic;
using DelauNET.Model;
using DelauNET.Triangulation;
using Xunit;
using Xunit.Abstractions;

namespace DelauNET.Tests
{
    public class CircumcenterTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CircumcenterTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var converter = new OutputConverter(_testOutputHelper);
            Console.SetOut(converter);
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[]
            {
                new Triangle(new Vertex(5, 10), new Vertex(9, 2), new Vertex(1, 2)),
                new Vertex(5, 5)
            };
            yield return new object[]
            {
                new Triangle(new Vertex(10, 20), new Vertex(18, 4), new Vertex(2, 4)),
                new Vertex(10, 10),
            };
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void SimpleCircumcenter(Triangle triangle, Vertex expected)
        {
            var actual = triangle.Circumcenter;
            _testOutputHelper.WriteLine("ACTUAL: " + actual);
            _testOutputHelper.WriteLine("EXPECTED: " + expected);
            Assert.Equal(expected, actual);
        }
    }
}