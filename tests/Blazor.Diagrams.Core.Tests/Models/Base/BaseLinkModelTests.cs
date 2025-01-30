using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Models.Base;

public class BaseLinkModelTests
{
    [Fact]
    public void SetSource_ShouldChangePropertiesAndTriggerEvent()
    {
        // Arrange
        var link = new LinkModel(sourcePort: new PortModel(null), targetPort: null);
        var parent = new NodeModel();
        var port = new PortModel(parent);
        var sp = new SinglePortAnchor(port);
        var eventsTriggered = 0;
        Anchor? oldSp = null;
        Anchor? newSp = null;
        BaseLinkModel? linkInstance = null;

        // Act
        link.SourceChanged += (l, o, n) =>
        {
            eventsTriggered++;
            linkInstance = l;
            oldSp = o;
            newSp = n;
        };

        link.SetSource(sp);

        // Assert
        Assert.Equal(1, eventsTriggered);
        Assert.Same(sp, link.Source);
        Assert.NotNull(oldSp);
        Assert.Same(sp, newSp);
        Assert.Same(link, linkInstance);
        Assert.Same(port, link.Source.Model);
    }

    [Fact]
    public void SetTarget_ShouldChangePropertiesAndTriggerEvent()
    {
        // Arrange
        var link = new LinkModel(new SinglePortAnchor(null), new PositionAnchor(Point.Zero));
        var parent = new NodeModel();
        var port = new PortModel(parent);
        var tp = new SinglePortAnchor(port);
        var eventsTriggered = 0;
        Anchor? oldTp = null;
        Anchor? newTp = null;
        BaseLinkModel? linkInstance = null;

        // Act
        link.TargetChanged += (l, o, n) =>
        {
            eventsTriggered++;
            linkInstance = l;
            oldTp = o;
            newTp = n;
        };

        link.SetTarget(tp);

        // Assert
        Assert.Equal(1, eventsTriggered);
        Assert.Same(tp, link.Target);
        Assert.IsType<PositionAnchor>(oldTp);
        Assert.Same(tp, newTp);
        Assert.Same(link, linkInstance);
        Assert.Same(port, link.Target!.Model);
    }

    [Fact]
    public void GetBounds_ShouldReturnPathBBox()
    {
        // Arrange
        var link = new LinkModel(new PositionAnchor(new Point(10, 5)), new PositionAnchor(new Point(100, 80)));
        link.Diagram = new TestDiagram();
        link.PathGenerator = new StraightPathGenerator();
        link.Router = new NormalRouter();

        // Act
        link.Refresh();
        var bounds = link.GetBounds()!;

        // Assert
        Assert.Equal(10, bounds.Left);
        Assert.Equal(5, bounds.Top);
        Assert.Equal(90, bounds.Width);
        Assert.Equal(75, bounds.Height);
    }
}
