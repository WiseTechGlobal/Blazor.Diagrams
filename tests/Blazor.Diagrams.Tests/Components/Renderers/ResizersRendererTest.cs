using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Bunit;
using FluentAssertions;
using Xunit;

namespace Blazor.Diagrams.Tests.Components.Renderers
{
    public class ResizersRendererTest
    {
        [Fact]
        public void ShouldRenderResizers()
        {
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;

            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, new BlazorDiagram()));

            component.MarkupMatches("<div class=\"resizers\"><div class=\"top-left my-resizer\"></div><div class=\"top-right my-resizer\"></div><div class=\"bottom-left my-resizer\"></div><div class=\"bottom-right my-resizer\"></div></div>");
        }

        [Fact]
        public void ShouldNotSeeResizersWhenNotSelected()
        {
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = false;

            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, new BlazorDiagram()));

            component.MarkupMatches("<div class=\"resizers\" style=\"visibility: hidden;\"><div class=\"top-left my-resizer\"></div><div class=\"top-right my-resizer\"></div><div class=\"bottom-left my-resizer\"></div><div class=\"bottom-right my-resizer\"></div></div>");
        }

        [Fact]
        public void ShouldRecalculateSizeAndPosition_TopLeft()
        {
            // setup
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));
            var resizer = component.Find(".top-left");
            resizer.PointerDown();
            var eventArgs = new PointerEventArgs(10, 15, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(10);
            node.Position.Y.Should().Be(15);
            node.Size.Width.Should().Be(90);
            node.Size.Height.Should().Be(185);
        }

        [Fact]
        public void ShouldRecalculateSizeAndPosition_TopRight()
        {
            // setup
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));
            var resizer = component.Find(".top-right");
            resizer.PointerDown();
            var eventArgs = new PointerEventArgs(10, 15, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(15);
            node.Size.Width.Should().Be(110);
            node.Size.Height.Should().Be(185);
        }

        [Fact]
        public void ShouldRecalculateSizeAndPosition_BottomLeft()
        {
            // setup
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));
            var resizer = component.Find(".bottom-left");
            resizer.PointerDown();
            var eventArgs = new PointerEventArgs(10, 15, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(10);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(90);
            node.Size.Height.Should().Be(215);
        }

        [Fact]
        public void ShouldRecalculateSizeAndPosition_BottomRight()
        {
            // setup
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));
            var resizer = component.Find(".bottom-right");
            resizer.PointerDown();
            var eventArgs = new PointerEventArgs(10, 15, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(110);
            node.Size.Height.Should().Be(215);
        }

        [Fact]
        public void ShouldStopResizingSmallerThanMinimumSize()
        {
            // setup
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            // before resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(100);
            node.Size.Height.Should().Be(200);

            // resize
            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));
            var resizer = component.Find(".top-left");
            resizer.PointerDown();
            var eventArgs = new PointerEventArgs(300, 300, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(0);
            node.Position.Y.Should().Be(0);
            node.Size.Width.Should().Be(node.MinimumDimensions.Width);
            node.Size.Height.Should().Be(node.MinimumDimensions.Height);
        }

        [Fact]
        public void NodeShouldNotResizeByDefault()
        {
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));

            // can't resize if there are no resizers
            component.FindAll(".my-resizer").Count.Should().Be(0);
        }

        [Fact]
        public void ShouldStopResizeOnPointerUp()
        {
            // setup
            using var ctx = new TestContext();
            var node = new NodeModel();
            node.ResizingEnabled = true;
            node.Selected = true;
            node.Size = new Size(100, 200);
            var diagram = new BlazorDiagram();

            // resize
            var component = ctx.RenderComponent<ResizersRenderer>(parameters => parameters
                .Add(n => n.Node, node)
                .Add(n => n.ResizerClass, "my-resizer")
                .Add(n => n.BlazorDiagram, diagram));
            var resizer = component.Find(".top-left");
            resizer.PointerDown();
            var eventArgs = new PointerEventArgs(10, 15, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // after resize
            node.Position.X.Should().Be(10);
            node.Position.Y.Should().Be(15);
            node.Size.Width.Should().Be(90);
            node.Size.Height.Should().Be(185);

            diagram.TriggerPointerUp(null, eventArgs);

            // mouve pointer after pointer up
            eventArgs = new PointerEventArgs(30, 50, 1, 1, false, false, false, 1, 1, 1, 1, 1, 1, "arrow", true);
            diagram.TriggerPointerMove(null, eventArgs);

            // should be no change
            node.Position.X.Should().Be(10);
            node.Position.Y.Should().Be(15);
            node.Size.Width.Should().Be(90);
            node.Size.Height.Should().Be(185);
        }
    }
}
