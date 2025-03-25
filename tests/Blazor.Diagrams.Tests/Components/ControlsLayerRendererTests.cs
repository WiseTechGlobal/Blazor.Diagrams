using Bunit;
using Xunit;
using Blazor.Diagrams.Components.Controls;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Tests.Components
{
    public class ControlsLayerRendererTests
    {
        [Fact]
        public async Task Rendering_WithChangingModels_ShouldNotThrowException()
        {
            // Arrange
            using var ctx = new TestContext();
            var diagram = new BlazorDiagram();

            var model1 = new CustomModel();
            var model2 = new CustomModel();

            diagram.Controls.AddFor(model1);
            diagram.Controls.AddFor(model2);

            var renderStarted = new ManualResetEventSlim(false);
            var modificationStarted = new ManualResetEventSlim(false);
            var exceptionThrown = false;

            IRenderedComponent<ControlsLayerRenderer>? cut = null;

            // Task to render the component
            var renderTask = Task.Run(async () =>
            {
                try
                {
                    cut = ctx.RenderComponent<ControlsLayerRenderer>(parameters => parameters
                        .Add(c => c.BlazorDiagram, diagram));

                    renderStarted.Set(); // Indicate that rendering has started

                    // Force Blazor to update while modifications happen
                    for (int i = 0; i < 10; i++)
                    {
                        await cut.InvokeAsync(() =>
                        {
                            cut.Render();
                        });
                        Thread.Sleep(5); // Let it process
                    }

                    modificationStarted.Wait(); // Wait for modifications
                }
                catch (InvalidOperationException)
                {
                    exceptionThrown = true;
                }
            });

            // Wait to ensure rendering starts first
            renderStarted.Wait();
            await Task.Delay(10); // Allow time for rendering

            // Task to modify the collection while rendering happens
            var modifyTask = Task.Run(async () =>
            {
                try
                {
                    foreach (var model in diagram.Controls.Models)
                    {
                        if (model == model1)
                        {
                            await cut.InvokeAsync(() =>
                            {
                                diagram.Controls.RemoveFor(model1);
                            });
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    exceptionThrown = true;
                }
                finally
                {
                    modificationStarted.Set();
                }
            });

            // Wait for tasks to complete
            await Task.WhenAll(renderTask, modifyTask);

            // Assert
            Assert.False(exceptionThrown, "Iteration should not throw an exception when using .ToList()");
        }
        private class CustomModel : Model { }
    }
}
