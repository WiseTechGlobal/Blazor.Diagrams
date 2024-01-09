using Xunit;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using System.Collections.Generic;

namespace Blazor.Diagrams.Core.Tests.Extensions;

public class DoubleExtensionsTests
{
    public static IEnumerable<object[]> AlmostEqualTo_TestData()
    {
        yield return new object[] { 5, 10, 0.1, false };
        yield return new object[] { 1.1, 1.2, 0.01, false };
        yield return new object[] { 10, 10, 0.0001, true };
        yield return new object[] { 10.35, 10.35, 0.0001, true };
        yield return new object[] { 1.659, 1.660, 0.0001, false };
        yield return new object[] { 1.65999, 1.65998, 0.0001, true };
        yield return new object[] { 1.65999, 1.6599998, 0.0001, true };
    }

    [Theory(DisplayName = "AlmostEqualTo")]
    [MemberData(nameof(AlmostEqualTo_TestData))]
    public void AlmostEqualTo(double num1, double num2, double tolerance, bool expected)
    {
        Assert.Equal(expected, num1.AlmostEqualTo(num2, tolerance));
    }
}
