using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Components.Renderers;

public class ResizersRenderer : ComponentBase, IDisposable
{
    private bool _shouldRender = true;

    [Parameter] public NodeModel Node { get; set; } = null!;
    [Parameter] public string? ResizerClass { get; set; } = "default-resizer";
    string ResizersClass = "resizers";

    public void Dispose()
    {
    }

    protected override bool ShouldRender()
    {
        if (!_shouldRender)
            return false;

        _shouldRender = false;
        return true;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Node.ResizingEnabled)
            return;

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", ResizersClass);

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"top-left {ResizerClass}");
        builder.CloseElement();

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"top-right {ResizerClass}");
        builder.CloseElement();

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"bottom-left {ResizerClass}");
        builder.CloseElement();

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"bottom-right {ResizerClass}");
        builder.CloseElement();

        builder.CloseElement();
    }
}
