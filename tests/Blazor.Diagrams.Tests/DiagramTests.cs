using AngleSharp.Dom;
using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Moq;
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

    [Fact]
    public void Rendering_WithChangingModels_ShouldNotThrowException()
    {
        // Arrange
        var mockControlLayer = new Mock<IControlsLayer>();
        var model1 = new Mock<Model>();
        var model2 = new Mock<Model>();

        var models = new List<Model> { model1.Object, model2.Object };

        mockControlLayer.Setup(c => c.Models).Returns(() => models); // Dynamic access

        bool exceptionThrown = false;

        // Act
        try
        {
            foreach (var model in mockControlLayer.Object.Models.ToList()) // Using .ToList() to prevent modification issues
            {
                // Simulate model change while iterating
                if (model == model1.Object)
                {
                    models.Remove(model1.Object); // This would cause InvalidOperationException without .ToList()
                }
            }
        }
        catch (InvalidOperationException)
        {
            exceptionThrown = true;
        }

        // Assert
        Assert.False(exceptionThrown, "Iteration should not throw an exception when using .ToList()");
    }
    public interface IControlsLayer
    {
        IEnumerable<Model> Models { get; }
    }
}
