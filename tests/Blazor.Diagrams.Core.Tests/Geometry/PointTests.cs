using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Geometry;

public class PointTests
{
    public static IEnumerable<object[]> DistanceTo_TestData()
    {
        yield return new object[] { 0, 0, 0, 0, 0 };
        yield return new object[] { -7, -4, 17, 6.5, 26.196374 };
        yield return new object[] { 5, 10, 33, 98, 92.347171 };
        yield return new object[] { 5.5, 2.7, 6.5, 47.2, 44.511235 };
    }

    [Theory(DisplayName = "DistanceTo")]
    [MemberData(nameof(DistanceTo_TestData))]
    public void DistanceTo(double x1, double y1, double x2, double y2, double expected)
    {
        var pt1 = new Point(x1, y1);
        var pt2 = new Point(x2, y2);
        pt1.DistanceTo(pt2).Should().BeApproximately(expected, 0.0001);
    }
}
