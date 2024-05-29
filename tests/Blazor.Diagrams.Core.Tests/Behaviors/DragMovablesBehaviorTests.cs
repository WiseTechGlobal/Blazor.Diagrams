using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Options;
using FluentAssertions;
using Moq;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors;

public class DragMovablesBehaviorTests
{
    [Fact]
    public void Behavior_ShouldCallSetPosition()
    {
        // Arrange
        var diagram = new TestDiagram();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var node = diagram.Nodes.Add(nodeMock.Object);
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        nodeMock.Verify(n => n.SetPosition(50, 50), Times.Once);
    }

    [Fact]
    public void Behavior_ShouldTriggerMoved()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel(Point.Zero));
        var movedTrigger = false;
        node.Moved += m => movedTrigger = true;
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        movedTrigger.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldNotTriggerMoved_WhenMovableDidntMove()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel(Point.Zero));
        var movedTrigger = false;
        node.Moved += m => movedTrigger = true;
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerUp(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        movedTrigger.Should().BeFalse();
    }

    [Fact]
    public void Behavior_ShouldCallSetPosition_WhenGroupHasAutoSize()
    {
        // Arrange
        var diagram = new TestDiagram();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var group = new GroupModel(new[] { nodeMock.Object }, autoSize: true);
        var node = diagram.Nodes.Add(nodeMock.Object);
        diagram.SelectModel(node, false);

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerPointerMove(null,
            new PointerEventArgs(150, 150, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));

        // Assert
        nodeMock.Verify(n => n.SetPosition(50, 50), Times.Once);
    }

    [Fact]
    public void Behavior_ShouldCallSetPosition_WhenPanChanges()
    {
        // Arrange
        var diagram = new TestDiagram();
        var nodeMock = new Mock<NodeModel>(Point.Zero);
        var node = diagram.Nodes.Add(nodeMock.Object);
        diagram.SelectModel(node, false);
        diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ScrollBehavior>();
        diagram.SetContainer(new Rectangle(0, 0, 100, 100));

        // Act
        diagram.TriggerPointerDown(node,
            new PointerEventArgs(100, 100, 0, 0, false, false, false, 0, 0, 0, 0, 0, 0, string.Empty, true));
        diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 100, 100, 0, 0));

        // Assert
        nodeMock.Verify(n => n.SetPosition(100, 100), Times.Once);
    }
}