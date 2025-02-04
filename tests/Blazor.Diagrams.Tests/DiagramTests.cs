using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Blazor.Diagrams.Tests;

public class DiagramTests
{
    [Fact]
    public void GetComponentForModel_ShouldReturnComponentType_WhenModelTypeWasRegistered()
    {
        // Arrange
        var diagram = new BlazorDiagram();
        diagram.RegisterComponent<NodeModel, NodeWidget>();

        // Act
        var componentType = diagram.GetComponent<NodeModel>();

        // Assert
        Assert.Equal(typeof(NodeWidget), componentType);
    }

    [Fact]
    public void GetComponentForModel_ShouldReturnNull_WhenModelTypeWasNotRegistered()
    {
        // Arrange
        var diagram = new BlazorDiagram();

        // Act
        var componentType = diagram.GetComponent<NodeModel>();

        // Assert
        Assert.Null(componentType);
    }

    [Fact]
    public void GetComponentForModel_ShouldReturnComponentType_WhenInheritedModelTypeWasRegistered()
    {
        // Arrange
        var diagram = new BlazorDiagram();
        diagram.RegisterComponent<Model, NodeWidget>();

        // Act
        var componentType = diagram.GetComponent<CustomModel>();

        // Assert
        Assert.Equal(typeof(NodeWidget), componentType);
    }

    [Fact]
    public void GetComponentForModel_ShouldReturnSpecificComponentType_WhenInheritedAndSpecificModelTypeWasRegistered()
    {
        // Arrange
        var diagram = new BlazorDiagram();
        diagram.RegisterComponent<CustomModel, CustomWidget>();
        diagram.RegisterComponent<Model, NodeWidget>();

        // Act
        var componentType = diagram.GetComponent<CustomModel>();

        // Assert
        Assert.Equal(typeof(CustomWidget), componentType);
    }

    [Fact]
    public void GetComponentForModel_ShouldReturnNull_WhenCheckSubclassesIsFalse()
    {
        // Arrange
        var diagram = new BlazorDiagram();
        diagram.RegisterComponent<Model, NodeWidget>();

        // Act
        var componentType = diagram.GetComponent<CustomModel>(false);

        // Assert
        Assert.Null(componentType);
    }

    private class CustomModel : Model { }
    private class CustomWidget : ComponentBase { }
}
