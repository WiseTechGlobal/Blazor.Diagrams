using Blazor.Diagrams.Core.Geometry;

namespace Blazor.Diagrams.Core.Models.Base;

internal interface IResizable
{
    public bool ResizingEnabled { get; set; }
    public bool IsCurrentlyResizing { get; set; }
    public ResizerPosition CurrentResizer { get; set; }

    public Point OriginalPosition { get; set; }
    public Point OriginalMousePosition { get; set; }
    public double OriginalWidth { get; set; }
    public double OriginalHeight { get; set; }

    public double CurrentWidth { get; set; }
    public double CurrentHeight { get; set; }
    public Size MinimumDimensions { get; set; }

}

public enum ResizerPosition
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft
}
