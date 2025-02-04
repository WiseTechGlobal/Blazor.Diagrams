﻿using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Positions;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Anchors;

public class DynamicAnchorTests
{
    [Fact]
    public void GetPosition_ShouldReturnNull_WhenNodesSizeIsNull()
    {
        // Arrange
        var node = new NodeModel(position: new Point(120, 95))
        {
            Size = null
        };
        var providers = new[]
        {
            new BoundsBasedPositionProvider(0.0, 0.0),
            new BoundsBasedPositionProvider(0.5, 0.0),
            new BoundsBasedPositionProvider(1.0, 0.0),
            new BoundsBasedPositionProvider(0.0, 0.5),
            new BoundsBasedPositionProvider(0.5, 0.5),
            new BoundsBasedPositionProvider(1.0, 0.5),
            new BoundsBasedPositionProvider(0.0, 1.0),
            new BoundsBasedPositionProvider(0.5, 1.0),
            new BoundsBasedPositionProvider(1.0, 1.0)
        };
        var anchor1 = new DynamicAnchor(node, providers);
        var anchor2 = new DynamicAnchor(node, providers);
        var link = new LinkModel(anchor1, anchor2);

        // Act
        var position = anchor1.GetPosition(link);

        // Assert
        Assert.Null(position);
    }

    [Fact]
    public void GetPosition_ShouldReturnClosestPositionToOtherNodesCenter_WhenRouteIsEmpty()
    {
        // Arrange
        var node1 = new NodeModel(position: new Point(120, 95))
        {
            Size = new Size(100, 60)
        };
        var node2 = new NodeModel(position: new Point(200, 60))
        {
            Size = new Size(100, 60)
        };
        var positions = new[]
        {
            new BoundsBasedPositionProvider(0.0, 0.0), // 120,95
            new BoundsBasedPositionProvider(0.5, 0.0), // 170,95
            new BoundsBasedPositionProvider(1.0, 0.0), // 220,95
            new BoundsBasedPositionProvider(0.0, 0.5), // 120,125
            new BoundsBasedPositionProvider(0.5, 0.5), // 170,125
            new BoundsBasedPositionProvider(1.0, 0.5), // 220,125
            new BoundsBasedPositionProvider(0.0, 1.0), // 120,155
            new BoundsBasedPositionProvider(0.5, 1.0), // 170,155
            new BoundsBasedPositionProvider(1.0, 1.0) // 220,155
        };
        var anchor1 = new DynamicAnchor(node1, positions);
        var anchor2 = new DynamicAnchor(node2, positions);
        var link = new LinkModel(anchor1, anchor2);

        // Act
        var position = anchor1.GetPosition(link);

        // Assert
        Assert.NotNull(position);
        Assert.Equal(220,position!.X);
        Assert.Equal(95, position.Y);
    }

    [Fact]
    public void GetPosition_ShouldReturnClosestPositionToOtherNodesCenterWithOffset_WhenRouteIsEmpty()
    {
        // Arrange
        var node1 = new NodeModel(position: new Point(120, 95))
        {
            Size = new Size(100, 60)
        };
        var node2 = new NodeModel(position: new Point(200, 60))
        {
            Size = new Size(100, 60)
        };
        var positions = new[]
        {
            new BoundsBasedPositionProvider(0.0, 0.0), // 120,95
            new BoundsBasedPositionProvider(0.5, 0.0), // 170,95
            new BoundsBasedPositionProvider(1.0, 0.0, 10, -10), // 230,85
            new BoundsBasedPositionProvider(0.0, 0.5), // 120,125
            new BoundsBasedPositionProvider(0.5, 0.5), // 170,125
            new BoundsBasedPositionProvider(1.0, 0.5), // 220,125
            new BoundsBasedPositionProvider(0.0, 1.0), // 120,155
            new BoundsBasedPositionProvider(0.5, 1.0), // 170,155
            new BoundsBasedPositionProvider(1.0, 1.0) // 220,155
        };
        var anchor1 = new DynamicAnchor(node1, positions);
        var anchor2 = new DynamicAnchor(node2, positions);
        var link = new LinkModel(anchor1, anchor2);

        // Act
        var position = anchor1.GetPosition(link);

        // Assert
        Assert.NotNull(position);
        Assert.Equal(230, position!.X);
        Assert.Equal(85, position.Y);
    }

    [Fact]
    public void GetPosition_ShouldReturnClosestPositionToFirstVertex_WhenRouteIsNotEmpty()
    {
        // Arrange
        var node1 = new NodeModel(position: new Point(120, 95))
        {
            Size = new Size(100, 60)
        };
        var node2 = new NodeModel(position: new Point(300, 60))
        {
            Size = new Size(100, 60)
        };
        var positions = new[]
        {
            new BoundsBasedPositionProvider(0.0, 0.0), // 120,95
            new BoundsBasedPositionProvider(0.5, 0.0), // 170,95
            new BoundsBasedPositionProvider(1.0, 0.0), // 220,95
            new BoundsBasedPositionProvider(0.0, 0.5), // 120,125
            new BoundsBasedPositionProvider(0.5, 0.5), // 170,125
            new BoundsBasedPositionProvider(1.0, 0.5), // 220,125
            new BoundsBasedPositionProvider(0.0, 1.0), // 120,155
            new BoundsBasedPositionProvider(0.5, 1.0), // 170,155
            new BoundsBasedPositionProvider(1.0, 1.0) // 220,155
        };
        var anchor1 = new DynamicAnchor(node1, positions);
        var anchor2 = new DynamicAnchor(node2, positions);
        var link = new LinkModel(anchor1, anchor2);

        // Act
        var position = anchor1.GetPosition(link, new Point[]
        {
            new Point(280, 115) // Vertex
        });

        // Assert
        Assert.NotNull(position);
        Assert.Equal(220, position!.X);
        Assert.Equal(125, position.Y);
    }

    [Fact]
    public void GetPosition_ShouldReturnClosestPositionToLastVertex_WhenRouteIsNotEmptyAndIsTarget()
    {
        // Arrange
        var node1 = new NodeModel(position: new Point(120, 95))
        {
            Size = new Size(100, 60)
        };
        var node2 = new NodeModel(position: new Point(300, 60))
        {
            Size = new Size(100, 60)
        };
        var positions = new[]
        {
            new BoundsBasedPositionProvider(0.0, 0.0), // 300, 60
            new BoundsBasedPositionProvider(0.5, 0.0), // 350, 60
            new BoundsBasedPositionProvider(1.0, 0.0), // 400, 60
            new BoundsBasedPositionProvider(0.0, 0.5), // 300, 90
            new BoundsBasedPositionProvider(0.5, 0.5), // 350, 90
            new BoundsBasedPositionProvider(1.0, 0.5), // 400, 90
            new BoundsBasedPositionProvider(0.0, 1.0), // 300, 120
            new BoundsBasedPositionProvider(0.5, 1.0), // 350, 120
            new BoundsBasedPositionProvider(1.0, 1.0) // 400, 120
        };
        var anchor1 = new DynamicAnchor(node1, positions);
        var anchor2 = new DynamicAnchor(node2, positions);
        var link = new LinkModel(anchor1, anchor2);

        // Act
        var position = anchor2.GetPosition(link, new Point[]
        {
            new Point(280, 115) // Vertex
        });

        // Assert
        Assert.NotNull(position);
        Assert.Equal(300, position!.X);
        Assert.Equal(120, position.Y);
    }
}
