﻿using Blazor.Diagrams.Core.Models;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Layers;

public class NodeLayerTests
{
    [Fact]
    public void Remove_ShouldRemoveAllPortLinks()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel());
        var nodePort1 = node1.AddPort(PortAlignment.Top);
        var node2 = diagram.Nodes.Add(new NodeModel());
        var nodePort2 = node2.AddPort(PortAlignment.Top);
        diagram.Links.Add(new LinkModel(nodePort1, nodePort2));

        // Act
        diagram.Nodes.Remove(node1);

        // Assert
        Assert.Empty(diagram.Links);
    }

    [Fact]
    public void Remove_ShouldRemoveAllLinks()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel());
        var node2 = diagram.Nodes.Add(new NodeModel());
        diagram.Links.Add(new LinkModel(node1, node2));

        // Act
        diagram.Nodes.Remove(node1);

        // Assert
        Assert.Empty(diagram.Links);
    }

    [Fact]
    public void Remove_ShouldRemoveItselfFromParentGroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));

        // Act
        diagram.Nodes.Remove(node);

        // Assert
        Assert.Empty(group.Children);
        Assert.Null(node.Group);
    }

    [Fact]
    public void Add_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        var node = diagram.Nodes.Add(new NodeModel());

        // Assert
        Assert.Equal(1,refreshes);
    }

    [Fact]
    public void Remove_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node1 = diagram.Nodes.Add(new NodeModel());
        var node2 = diagram.Nodes.Add(new NodeModel());
        diagram.Links.Add(new LinkModel(node1, node2));
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        diagram.Nodes.Remove(node1);

        // Assert
        Assert.Equal(1,refreshes);
    }

    [Fact]
    public void Remove_ShouldRemoveControls()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var controls = diagram.Controls.AddFor(node);

        // Act
        diagram.Nodes.Remove(node);

        // Assert
        Assert.Null(diagram.Controls.GetFor(node));
    }
}
