using Blazor.Diagrams.Components.Controls;
using Blazor.Diagrams.Components.Renderers;
using Bunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Tests.Components.Controls
{
    public class ResizeControlWidgetTests
    {
        [Fact]
        public void ShouldRenderDiv()
        {
            using var ctx = new TestContext();

            var cut = ctx.RenderComponent<ResizeControlWidget>();

            cut.MarkupMatches("<div class=\"default-node-resizer\" />");
        }
    }
}
