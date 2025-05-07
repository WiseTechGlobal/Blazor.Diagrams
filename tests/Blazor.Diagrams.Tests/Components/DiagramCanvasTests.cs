using Blazor.Diagrams.Components;
using Blazor.Diagrams.Core.Geometry;
using Bunit;
using Microsoft.JSInterop;
using Xunit;

namespace Blazor.Diagrams.Tests.Components
{
    public class DiagramCanvasTests
    {
        [Fact]
        public void Behavior_WhenDisposing_ShouldUnsubscribeToResizes()
        {
            // Arrange
            JSRuntimeInvocationHandler call;
            using (var ctx = new TestContext())
            {
                ctx.JSInterop.Setup<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", _ => true);
                call = ctx.JSInterop.SetupVoid("ZBlazorDiagrams.unobserve", _ => true).SetVoidResult();

                // Act
                var cut = ctx.RenderComponent<DiagramCanvas>(p => p.Add(n => n.BlazorDiagram, new BlazorDiagram()));
            }

            // Assert
            call.VerifyInvoke("ZBlazorDiagrams.unobserve", calledTimes: 1);
        }

        [Theory]
        [MemberData(nameof(ExceptionTestData))]
        public async Task DisposeAsync_ShouldCaptureExpectedExceptions(Exception exceptionToThrow)
        {
            // Arrange
            using var ctx = new TestContext();
            ctx.JSInterop.Setup<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", _ => true);
            ctx.JSInterop.SetupVoid("ZBlazorDiagrams.unobserve", _ => true).SetException(exceptionToThrow);

            // Act
            var cut = ctx.RenderComponent<DiagramCanvas>(p => p.Add(n => n.BlazorDiagram, new BlazorDiagram()));
            var exception = await Record.ExceptionAsync(async () => await cut.Instance.DisposeAsync());

            // Assert
            Assert.Null(exception);
        }

        public static IEnumerable<object[]> ExceptionTestData =>
            new List<object[]>
            {
                new object[] { new TaskCanceledException() },
                new object[] { new JSDisconnectedException("JSDisconnectedException") }
            };
    }
}
