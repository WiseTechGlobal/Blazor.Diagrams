using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointerEventArgs = Microsoft.AspNetCore.Components.Web.PointerEventArgs;

namespace Blazor.Diagrams.Components.Renderers;

public class ResizersRenderer : ComponentBase, IDisposable
{
    private bool _shouldRender = true;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; } = null!;
    [Parameter] public NodeModel Node { get; set; } = null!;
    [Parameter] public string ResizerClass { get; set; } = "default-resizer";
    string ResizersClass = "resizers";

    [Parameter] public Action<Size>? OnSizeChange { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Node.SelectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(SelectableModel m)
    {
        _shouldRender = true;
        StateHasChanged();
    }

    public void Dispose()
    {
        Node.SelectionChanged -= OnSelectionChanged;
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
        if (Node.OnlyResizeWhenSelected && !Node.Selected)
        {
            builder.AddAttribute(2, "style", "visibility: hidden;");
        }

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"top-left {ResizerClass}");
        builder.AddAttribute(2, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, (args) => OnPointerDown(ResizerPosition.TopLeft, args)));
        builder.AddEventStopPropagationAttribute(3, "onpointerdown", true);
        builder.CloseElement();

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"top-right {ResizerClass}");
        builder.AddAttribute(2, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, (args) => OnPointerDown(ResizerPosition.TopRight, args)));
        builder.AddEventStopPropagationAttribute(3, "onpointerdown", true);
        builder.CloseElement();

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"bottom-left {ResizerClass}");
        builder.AddAttribute(2, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, (args) => OnPointerDown(ResizerPosition.BottomLeft, args)));
        builder.AddEventStopPropagationAttribute(3, "onpointerdown", true);
        builder.CloseElement();

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", $"bottom-right {ResizerClass}");
        builder.AddAttribute(2, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, (args) => OnPointerDown(ResizerPosition.BottomRight, args)));
        builder.AddEventStopPropagationAttribute(3, "onpointerdown", true);
        builder.CloseElement();

        builder.CloseElement();
    }

    private void OnPointerDown(ResizerPosition resizerPosition, PointerEventArgs e)
    {
        Node.CurrentResizer = resizerPosition;
        Node.OriginalPosition = new Point(Node.Position.X, Node.Position.Y);
        Node.OriginalMousePosition = new Point(e.ClientX, e.ClientY);
        Node.OriginalWidth = Node.Size!.Width;
        Node.OriginalHeight = Node.Size!.Height;

        BlazorDiagram.PointerMove += Resize;
        BlazorDiagram.PointerUp += StopResize;
    }

    void Resize(Model? model, Core.Events.PointerEventArgs args)
    {
        var width = Node.OriginalWidth;
        var height = Node.OriginalHeight;
        var positionX = Node.Position.X;
        var positionY = Node.Position.Y;

        if (Node.CurrentResizer == ResizerPosition.TopLeft)
        {
            height = Node.OriginalHeight - (args.ClientY - Node.OriginalMousePosition.Y);
            width = Node.OriginalWidth - (args.ClientX - Node.OriginalMousePosition.X);

            positionX = Node.OriginalPosition.X + (args.ClientX - Node.OriginalMousePosition.X);
            positionY = Node.OriginalPosition.Y + (args.ClientY - Node.OriginalMousePosition.Y);
        }
        else if (Node.CurrentResizer == ResizerPosition.TopRight)
        {
            height = Node.OriginalHeight - (args.ClientY - Node.OriginalMousePosition.Y);
            width = Node.OriginalWidth + (args.ClientX - Node.OriginalMousePosition.X);

            positionX = Node.OriginalPosition.X;
            positionY = Node.OriginalPosition.Y + (args.ClientY - Node.OriginalMousePosition.Y);
        }
        else if (Node.CurrentResizer == ResizerPosition.BottomLeft)
        {
            height = Node.OriginalHeight + (args.ClientY - Node.OriginalMousePosition.Y);
            width = Node.OriginalWidth - (args.ClientX - Node.OriginalMousePosition.X);

            positionX = Node.OriginalPosition.X + (args.ClientX - Node.OriginalMousePosition.X);
            positionY = Node.OriginalPosition.Y;
        }
        else if (Node.CurrentResizer == ResizerPosition.BottomRight)
        {
            height = Node.OriginalHeight + (args.ClientY - Node.OriginalMousePosition.Y);
            width = Node.OriginalWidth + (args.ClientX - Node.OriginalMousePosition.X);
        }

        if (width < Node.MinimumDimensions.Width)
        {
            width = Node.MinimumDimensions.Width;
            positionX = Node.Position.X;
        }
        if (height < Node.MinimumDimensions.Height)
        {
            height = Node.MinimumDimensions.Height;
            positionY = Node.Position.Y;
        }

        Node.SetPosition(positionX, positionY);
        Node.Size = new Size(width, height);

        OnSizeChange?.Invoke(Node.Size);
        Node.Refresh();
    }

    void StopResize(Model? model, Core.Events.PointerEventArgs args)
    {
        BlazorDiagram.PointerMove -= Resize;
        BlazorDiagram.PointerUp -= StopResize;
    }
}
