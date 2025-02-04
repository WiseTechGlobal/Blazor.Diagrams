using Blazor.Diagrams.Components;
using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

using Bunit;


using Xunit;

namespace Blazor.Diagrams.Tests.Components;

public class NodeWidgetTests
{
    [Fact]
    public void DefaultNodeWidget_ShouldHaveSingleClassAndNoPorts_WhenItHasNoPortsAndNoSelectionNorGroup()
    {
        // Arrange
        using var ctx = new TestContext();
        var node = new NodeModel(Point.Zero);

        // Act
        var cut = ctx.RenderComponent<NodeWidget>(parameters => parameters
            .Add(n => n.Node, node));

        // Assert
        var content = cut.Find("div.default-node");
        Assert.Single(content.ClassList);
        Assert.Equal("default-node", content.ClassList[0]);
        Assert.Equal("Title", content.TextContent.Trim());

        var ports = cut.FindComponents<PortRenderer>();
        Assert.Empty(ports);
    }
}
