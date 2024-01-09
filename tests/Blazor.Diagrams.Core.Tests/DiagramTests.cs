﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Blazor.Diagrams.Core.Tests;

public class DiagramTests
{
    [Fact]
    public void GetScreenPoint_ShouldReturnCorrectPoint()
    {
        // Arrange
        var diagram = new TestDiagram();

        // Act
        diagram.SetZoom(1.234);
        diagram.UpdatePan(50, 50);
        diagram.SetContainer(new Rectangle(30, 65, 1000, 793));
        var pt = diagram.GetScreenPoint(100, 200);

        // Assert
        pt.X.Should().Be(203.4); // 2*X + panX + left
        pt.Y.Should().Be(361.8); // 2*Y + panY + top
    }

    [Fact]
    public void ZoomToFit_ShouldUseSelectedNodesIfAny()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(new Point(0, 0), new Size(1080, 768)));
        diagram.Nodes.Add(new NodeModel(new Point(50, 50))
        {
            Size = new Size(100, 80)
        });
        diagram.SelectModel(diagram.Nodes[0], true);

        // Act
        diagram.ZoomToFit(10);

        // Assert
        diagram.Zoom.Should().BeApproximately(7.68, 0.001);
        diagram.Pan.X.Should().Be(-307.2);
        diagram.Pan.Y.Should().Be(-307.2);
    }

    [Fact]
    public void ZoomToFit_ShouldUseNodesWhenNoneSelected()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(new Point(0, 0), new Size(1080, 768)));
        diagram.Nodes.Add(new NodeModel(new Point(50, 50))
        {
            Size = new Size(100, 80)
        });

        // Act
        diagram.ZoomToFit(10);

        // Assert
        diagram.Zoom.Should().BeApproximately(7.68, 0.001);
        diagram.Pan.X.Should().Be(-307.2);
        diagram.Pan.Y.Should().Be(-307.2);
    }

    [Fact]
    public void ZoomToFit_ShouldTriggerAppropriateEvents()
    {
        // Arrange
        var diagram = new TestDiagram();
        diagram.SetContainer(new Rectangle(new Point(0, 0), new Size(1080, 768)));
        diagram.Nodes.Add(new NodeModel(new Point(50, 50))
        {
            Size = new Size(100, 80)
        });

        var refreshes = 0;
        var zoomChanges = 0;
        var panChanges = 0;

        // Act
        diagram.Changed += () => refreshes++;
        diagram.ZoomChanged += () => zoomChanges++;
        diagram.PanChanged += () => panChanges++;
        diagram.ZoomToFit(10);

        // Assert
        refreshes.Should().Be(1);
        zoomChanges.Should().Be(1);
        panChanges.Should().Be(1);
    }

    public static IEnumerable<object[]> Zoom_ShoulClampToMinimumValue_TestData()
    {
        yield return new object[] { 0.001 };
        yield return new object[] { 0.1 };
    }

    [Theory(DisplayName = "Zoom_ShoulClampToMinimumValue")]
    [MemberData(nameof(Zoom_ShoulClampToMinimumValue_TestData))]
    public void Zoom_ShoulClampToMinimumValue(double zoomValue)
    {
        var diagram = new TestDiagram();
        diagram.SetZoom(zoomValue);
        Assert.Equal(diagram.Zoom, diagram.Options.Zoom.Minimum);
    }

    public static IEnumerable<object[]> ThrowExceptionWhenLessThan0_TestData()
    {
        yield return new object[] { 0 };
        yield return new object[] { -0.1 };
        yield return new object[] { -0.00001 };
    }

    [Theory(DisplayName = "Zoom_ThrowExceptionWhenLessThan0")]
    [MemberData(nameof (ThrowExceptionWhenLessThan0_TestData))]
    public void Zoom_ThrowExceptionWhenLessThan0(double zoomValue)
    {
        var diagram = new TestDiagram();
        Assert.Throws<ArgumentException>(() => diagram.SetZoom(zoomValue));
    }

    [Theory(DisplayName = "ZoomOptions_ThrowExceptionWhenLessThan0")]
    [MemberData(nameof(ThrowExceptionWhenLessThan0_TestData))]
    public void ZoomOptions_ThrowExceptionWhenLessThan0(double zoomValue)
    {
        var diagram = new TestDiagram();
        Assert.Throws<ArgumentException>(() => diagram.Options.Zoom.Minimum = zoomValue);
    }
        
    [Fact]
    public void SetContainer_ShouldAcceptNullGracefully()
    {
        // Arrange
        var diagram = new TestDiagram();

        //Act
        var exception = Record.Exception(() => diagram.SetContainer(null));

        // Assert
        exception.Should().BeNull();
    }
}
