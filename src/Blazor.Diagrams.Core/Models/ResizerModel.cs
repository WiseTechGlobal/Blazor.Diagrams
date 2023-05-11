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
    }

    public NodeModel Parent { get; }
    public PortAlignment Alignment { get; }
}
