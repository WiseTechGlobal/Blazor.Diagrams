using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Controls.Default;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Positions.Resizing;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Positions.Resizing;

public class BottomRightResizerProviderTests
{
    [Fact]
    public void DragResizer_ShouldResizeNode()
    {
        // setup
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(0, 0));
        node.Size = new Size(100, 200);
        var control = new ResizeControl(new BottomRightResizerProvider());
        diagram.Controls.AddFor(node).Add(control);
        diagram.SelectModel(node, false);

        // before resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(100,node.Size.Width);
        Assert.Equal(200,node.Size.Height);

        // resize
        var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        control.OnPointerDown(diagram, node, eventArgs);
        eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        diagram.TriggerPointerMove(null, eventArgs);

        // after resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(110,node.Size.Width);
        Assert.Equal(215,node.Size.Height);
    }

    [Fact]
    public void PanChanged_ShouldResizeNode()
    {
        // setup
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(0, 0));
        node.Size = new Size(100, 200);
        var control = new ResizeControl(new BottomRightResizerProvider());
        diagram.Controls.AddFor(node).Add(control);
        diagram.SelectModel(node, false);
        diagram.BehaviorOptions.DiagramWheelBehavior = diagram.GetBehavior<ScrollBehavior>();


        // before resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(100,node.Size.Width);
        Assert.Equal(200,node.Size.Height);

        // resize
        var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        control.OnPointerDown(diagram, node, eventArgs);
        diagram.TriggerWheel(new WheelEventArgs(100, 100, 0, 0, false, false, false, 10, 100, 0, 0));


        // after resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(110,node.Size.Width);
        Assert.Equal(300,node.Size.Height);
    }

    [Fact]
    public void DragResizer_SmallerThanMinSize_SetsNodeToMinSize()
    {
        // setup
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(0, 0));
        node.Size = new Size(100, 200);
        var control = new ResizeControl(new BottomRightResizerProvider());
        diagram.Controls.AddFor(node).Add(control);
        diagram.SelectModel(node, false);

        // before resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(100,node.Size.Width);
        Assert.Equal(200,node.Size.Height);

        // resize
        var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        control.OnPointerDown(diagram, node, eventArgs);
        eventArgs = new PointerEventArgs(-300, -300, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        diagram.TriggerPointerMove(null, eventArgs);

        // after resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(0,node.Size.Width);
        Assert.Equal(0,node.Size.Height);
    }

    [Fact]
    public void DragResizer_ShouldResizeNode_WhenDiagramZoomedOut()
    {
        // setup
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(0, 0));
        node.Size = new Size(100, 200);
        var control = new ResizeControl(new BottomRightResizerProvider());
        diagram.Controls.AddFor(node).Add(control);
        diagram.SelectModel(node, false);
        diagram.SetZoom(0.5);

        // before resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(100,node.Size.Width);
        Assert.Equal(200,node.Size.Height);

        // resize
        var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        control.OnPointerDown(diagram, node, eventArgs);
        eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        diagram.TriggerPointerMove(null, eventArgs);

        // after resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(120,node.Size.Width);
        Assert.Equal(230,node.Size.Height);
    }

    [Fact]
    public void DragResizer_ShouldResizeNode_WhenDiagramZoomedIn()
    {
        // setup
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(0, 0, 1000, 400));
        var node = new NodeModel(position: new Point(0, 0));
        node.Size = new Size(100, 200);
        var control = new ResizeControl(new BottomRightResizerProvider());
        diagram.Controls.AddFor(node).Add(control);
        diagram.SelectModel(node, false);
        diagram.SetZoom(2);

        // before resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(100,node.Size.Width);
        Assert.Equal(200,node.Size.Height);

        // resize
        var eventArgs = new PointerEventArgs(0, 0, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        control.OnPointerDown(diagram, node, eventArgs);
        eventArgs = new PointerEventArgs(10, 15, 0, 0, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
        diagram.TriggerPointerMove(null, eventArgs);

        // after resize
        Assert.Equal(0,node.Position.X);
        Assert.Equal(0,node.Position.Y);
        Assert.Equal(105,node.Size.Width);
        Assert.Equal(207.5,node.Size.Height);
    }
}
