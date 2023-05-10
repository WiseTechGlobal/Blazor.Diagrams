using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using System;

namespace Blazor.Diagrams.Core.Models;

public class ResizerModel : Model
{
    public ResizerModel(NodeModel parent, PortAlignment alignment)
    {
        Parent = parent;
        Alignment = alignment;
        MinimumDimensions = new Size(0, 0);
    }

    public NodeModel Parent { get; }
    public PortAlignment Alignment { get; }

    public bool ResizingEnabled { get; set; }
    public bool OnlyResizeWhenSelected { get; set; }

    public Size MinimumDimensions { get; set; }

    public Action<Size>? SizeChange { get; set; }
}
