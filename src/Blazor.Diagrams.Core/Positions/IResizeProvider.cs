﻿using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions
{
    public interface IResizeProvider : IPositionProvider
    {
        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs eventArgs);
        public void OnPointerMove(Model? model, PointerEventArgs args);
        public void OnResizeEnd(Model? model, PointerEventArgs args);
    }
}
