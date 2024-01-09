using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors;

public class KeyboardShortcutsBehaviorTests
{
    public static IEnumerable<object[]> Behavior_ShouldExecuteAction_WhenCombinationIsPressed_TestData()
    {
        yield return new object[] { "A", true, true, true };
        yield return new object[] { "B", true, false, false};
        yield return new object[] { "C", true, true, false};
        yield return new object[] { "D", true, false, true};
        yield return new object[] { "E", false, false, true};
        yield return new object[] { "F", false, true, true};
        yield return new object[] { "G", false, false, false};
    }

    [Theory(DisplayName = "Behavior_ShouldExecuteAction_WhenCombinationIsPressed")]
    [MemberData(nameof(Behavior_ShouldExecuteAction_WhenCombinationIsPressed_TestData))]
    public void Behavior_ShouldExecuteAction_WhenCombinationIsPressed(string key, bool ctrl, bool shift, bool alt)
    {
        // Arrange
        var diagram = new TestDiagram();
        var ksb = diagram.GetBehavior<KeyboardShortcutsBehavior>()!;
        var executed = false;

        ksb.SetShortcut(key, ctrl, shift, alt, d =>
        {
            executed = true;
            return ValueTask.CompletedTask;
        });

        // Act
        diagram.TriggerKeyDown(new KeyboardEventArgs(key, key, 0, ctrl, shift, alt));

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void Behavior_ShouldDoNothing_WhenRemoved()
    {
        // Arrange
        var diagram = new TestDiagram();
        var ksb = diagram.GetBehavior<KeyboardShortcutsBehavior>()!;
        diagram.UnregisterBehavior<KeyboardShortcutsBehavior>();
        var executed = false;

        ksb.SetShortcut("A", false, false, false, d =>
        {
            executed = true;
            return ValueTask.CompletedTask;
        });

        // Act
        diagram.TriggerKeyDown(new KeyboardEventArgs("A", "A", 0, false, false, false));

        // Assert
        executed.Should().BeFalse();
    }

    [Fact]
    public void SetShortcut_ShouldOverride()
    {
        // Arrange
        var diagram = new TestDiagram();
        var ksb = diagram.GetBehavior<KeyboardShortcutsBehavior>()!;
        var executed1 = false;
        var executed2 = false;

        ksb.SetShortcut("A", false, false, false, d =>
        {
            executed1 = true;
            return ValueTask.CompletedTask;
        });

        ksb.SetShortcut("A", false, false, false, d =>
        {
            executed2 = true;
            return ValueTask.CompletedTask;
        });

        // Act
        diagram.TriggerKeyDown(new KeyboardEventArgs("A", "A", 0, false, false, false));

        // Assert
        executed1.Should().BeFalse();
        executed2.Should().BeTrue();
    }
}
