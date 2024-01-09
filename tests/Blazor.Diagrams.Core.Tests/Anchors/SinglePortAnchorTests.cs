using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Anchors;

public class SinglePortAnchorTests
{
    [Fact]
    public void GetPlainPosition_ShouldReturnMiddlePosition()
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50)
        };
        var anchor = new SinglePortAnchor(port);

        // Act
        var position = anchor.GetPlainPosition()!;

        // Assert
        var mp = port.MiddlePosition;
        position.X.Should().Be(mp.X);
        position.Y.Should().Be(mp.Y);
    }

    [Fact]
    public void GetPosition_ShouldReturnNull_WhenPortNotInitialized()
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50)
        };
        var anchor = new SinglePortAnchor(port);
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link);

        // Assert
        position.Should().BeNull();
    }

    [Fact]
    public void GetPosition_ShouldReturnMiddlePosition_WhenMiddleIfNoMarker()
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50),
            Initialized = true
        };
        var anchor = new SinglePortAnchor(port)
        {
            MiddleIfNoMarker = true
        };
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link)!;

        // Assert
        var mp = port.MiddlePosition;
        position.X.Should().Be(mp.X);
        position.Y.Should().Be(mp.Y);
    }

    public static IEnumerable<object[]> GetPosition_ShouldReturnAlignmentBasedPosition_WhenUseShapeAndAlignmentIsFalse_TestData()
    {
        yield return new object[] { PortAlignment.Top, 110, 50 };
        yield return new object[] { PortAlignment.TopRight, 120, 50 };
        yield return new object[] { PortAlignment.Right, 120, 55 };
        yield return new object[] { PortAlignment.BottomRight, 120, 60 };
        yield return new object[] { PortAlignment.Bottom, 110, 60 };
        yield return new object[] { PortAlignment.BottomLeft, 100, 60 };
        yield return new object[] { PortAlignment.Left, 100, 55 };
        yield return new object[] { PortAlignment.TopLeft, 100, 50 };
    }

    [Theory(DisplayName = "GetPosition_ShouldReturnAlignmentBasedPosition_WhenUseShapeAndAlignmentIsFalse")]
    [MemberData(nameof(GetPosition_ShouldReturnAlignmentBasedPosition_WhenUseShapeAndAlignmentIsFalse_TestData))]
    public void GetPosition_ShouldReturnAlignmentBasedPosition_WhenUseShapeAndAlignmentIsFalse(PortAlignment alignment, double x, double y)
    {
        // Arrange
        var parent = new NodeModel();
        var port = new PortModel(parent, alignment)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50),
            Initialized = true
        };
        var anchor = new SinglePortAnchor(port)
        {
            MiddleIfNoMarker = false,
            UseShapeAndAlignment = false
        };
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link)!;

        // Assert
        position.X.Should().Be(x);
        position.Y.Should().Be(y);
    }

    public static IEnumerable<object[]> GetPosition_ShouldUsePointAtAngle_WhenUseShapeAndAlignmentIsTrue_TestData()
    {
        yield return new object[] { PortAlignment.Top, 270  };
        yield return new object[] { PortAlignment.TopRight, 315 };
        yield return new object[] { PortAlignment.Right, 0 };
        yield return new object[] { PortAlignment.BottomRight, 45 };
        yield return new object[] { PortAlignment.Bottom, 90 };
        yield return new object[] { PortAlignment.BottomLeft, 135 };
        yield return new object[] { PortAlignment.Left, 180 };
        yield return new object[] { PortAlignment.TopLeft, 225 };
    }

    [Theory(DisplayName = "GetPosition_ShouldUsePointAtAngle_WhenUseShapeAndAlignmentIsTrue")]
    [MemberData(nameof(GetPosition_ShouldUsePointAtAngle_WhenUseShapeAndAlignmentIsTrue_TestData))]
    public void GetPosition_ShouldUsePointAtAngle_WhenUseShapeAndAlignmentIsTrue(PortAlignment alignment, double angle)
    {
        // Arrange
        var parent = new NodeModel();
        var shapeMock = new Mock<IShape>();
        var port = new CustomPortModel(shapeMock.Object, parent, alignment)
        {
            Size = new Size(20, 10),
            Position = new Point(100, 50),
            Initialized = true
        };
        var anchor = new SinglePortAnchor(port)
        {
            MiddleIfNoMarker = false,
            UseShapeAndAlignment = true
        };
        var link = new LinkModel(anchor, new PositionAnchor(Point.Zero));

        // Act
        var position = anchor.GetPosition(link)!;
        
        // Assert
        shapeMock.Verify(s => s.GetPointAtAngle(angle), Times.Once);
    }

    private class CustomPortModel : PortModel
    {
        private readonly IShape _shape;

        public CustomPortModel(IShape shape, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(parent, alignment, position, size)
        {
            _shape = shape;
        }

        public CustomPortModel(IShape shape, string id, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(id, parent, alignment, position, size)
        {
            _shape = shape;
        }

        public override IShape GetShape() => _shape;
    }
}