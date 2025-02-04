using Blazor.Diagrams.Core.Models;
using System;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Layers;

public class GroupLayerTests
{
    [Fact]
    public void Group_ShouldCallFactoryThenAddMethod()
    {
        // Arrange
        var diagram = new TestDiagram();
        var factoryCalled = false;

        diagram.Options.Groups.Factory = (_, children) =>
        {
            factoryCalled = true;
            return new GroupModel(children);
        };

        // Act
        diagram.Groups.Group(Array.Empty<NodeModel>());

        // Assert
        Assert.True(factoryCalled);
    }

    [Fact]
    public void Remove_ShouldRemoveAllPortLinks()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var groupPort = group.AddPort(PortAlignment.Top);
        var node = diagram.Nodes.Add(new NodeModel());
        var nodePort = node.AddPort(PortAlignment.Top);
        diagram.Links.Add(new LinkModel(groupPort, nodePort));

        // Act
        diagram.Groups.Remove(group);

        // Assert
        Assert.Empty(diagram.Links);
    }

    [Fact]
    public void Remove_ShouldRemoveAllLinks()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var node = diagram.Nodes.Add(new NodeModel());
        diagram.Links.Add(new LinkModel(group, node));

        // Act
        diagram.Groups.Remove(group);

        // Assert
        Assert.Empty(diagram.Links);
    }

    [Fact]
    public void Remove_ShouldRemoveItselfFromParentGroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group1 = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var group2 = diagram.Groups.Add(new GroupModel(new[] { group1 }));

        // Act
        diagram.Groups.Remove(group1);

        // Assert
        Assert.Empty(group2.Children);
        Assert.Null(group1.Group);
    }

    [Fact]
    public void Remove_ShouldUngroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));

        // Act
        diagram.Groups.Remove(group);

        // Assert
        Assert.Empty(group.Children);
        Assert.Null(node.Group);
    }

    [Fact]
    public void Delete_ShouldDeleteChildGroup()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group1 = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var group2 = diagram.Groups.Add(new GroupModel(new[] { group1 }));

        // Act
        diagram.Groups.Delete(group2);

        // Assert
        Assert.Empty(diagram.Groups);
    }

    [Fact]
    public void Delete_ShouldRemoveChild()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));

        // Act
        diagram.Groups.Delete(group);

        // Assert
        Assert.Empty(diagram.Groups);
        Assert.Empty(diagram.Nodes);
    }

    [Fact]
    public void Add_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));

        // Assert
        Assert.Equal(1, refreshes);
    }

    [Fact]
    public void Remove_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var group = diagram.Groups.Add(new GroupModel(Array.Empty<NodeModel>()));
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        diagram.Groups.Remove(group);

        // Assert
        Assert.Equal(1,refreshes);
    }

    [Fact]
    public void Delete_ShouldRefreshDiagramOnce()
    {
        // Arrange
        var diagram = new TestDiagram();
        var node = diagram.Nodes.Add(new NodeModel());
        var group = diagram.Groups.Add(new GroupModel(new[] { node }));
        var refreshes = 0;
        diagram.Changed += () => refreshes++;

        // Act
        diagram.Groups.Delete(group);

        // Assert
        Assert.Equal(1,refreshes);
    }
}
