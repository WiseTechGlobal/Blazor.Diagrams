using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Components.Renderers;

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
    }
}
