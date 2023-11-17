﻿using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Positions.Resizing;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Core.Controls.Default
{
    public class ResizeControl : ExecutableControl
    {
        private readonly IResizeProvider _resizeProvider;

        public ResizeControl(IResizeProvider resizeProvider)
        {
            _resizeProvider = resizeProvider;
        }

        public override Point? GetPosition(Model model) => _resizeProvider.GetPosition(model);

        public override ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs e)
        {
            _resizeProvider.OnResizeStart(diagram, model, e);
            diagram.PointerMove += _resizeProvider.OnPointerMove;
            diagram.PointerUp += _resizeProvider.OnResizeEnd;

            return ValueTask.CompletedTask;
        }
    }
}
