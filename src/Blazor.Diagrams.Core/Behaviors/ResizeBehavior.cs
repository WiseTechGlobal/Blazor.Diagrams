using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Behaviors
{
    public class ResizeBehavior : Behavior
    {
        private double _originalWidth;
        private double _originalHeight;
        private Point _originalPosition = null!;
        private Point _originalMousePosition = null!;
        private ResizerModel? _resizer = null;

        public ResizeBehavior(Diagram diagram) : base(diagram)
        {
            Diagram.PointerDown += OnPointerDown;
            Diagram.PointerMove += OnPointerMove;
            Diagram.PointerUp += OnPointerUp;
        }

        private void OnPointerDown(Model? model, PointerEventArgs e)
        {
            if (model is not ResizerModel)
                return;

            _resizer = (ResizerModel)model;
            _originalPosition = new Point(_resizer.Parent.Position.X, _resizer.Parent.Position.Y);
            _originalMousePosition = new Point(e.ClientX, e.ClientY);
            _originalWidth = _resizer.Parent.Size!.Width;
            _originalHeight = _resizer.Parent.Size!.Height;
        }
        
        private void OnPointerMove(Model? model, PointerEventArgs args)
        {
            if (_resizer == null)
                return;
            Resize(_resizer.Alignment, _resizer.Parent, args);
        }

        private void OnPointerUp(Model? model, PointerEventArgs args)
        {
            _resizer = null;
        }

        void Resize(PortAlignment resizerAlignment, NodeModel model, PointerEventArgs args)
        {
            var width = _originalWidth;
            var height = _originalHeight;
            var positionX = model.Position.X;
            var positionY = model.Position.Y;

            if (resizerAlignment == PortAlignment.TopLeft)
            {
                height = _originalHeight - (args.ClientY - _originalMousePosition.Y);
                width = _originalWidth - (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
                positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);
            }
            else if (resizerAlignment == PortAlignment.TopRight)
            {
                height = _originalHeight - (args.ClientY - _originalMousePosition.Y);
                width = _originalWidth + (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X;
                positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);
            }
            else if (resizerAlignment == PortAlignment.BottomLeft)
            {
                height = _originalHeight + (args.ClientY - _originalMousePosition.Y);
                width = _originalWidth - (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
                positionY = _originalPosition.Y;
            }
            else if (resizerAlignment == PortAlignment.BottomRight)
            {
                height = _originalHeight + (args.ClientY - _originalMousePosition.Y);
                width = _originalWidth + (args.ClientX - _originalMousePosition.X);
            }
            else if (resizerAlignment == PortAlignment.Top)
            {
                height = _originalHeight - (args.ClientY - _originalMousePosition.Y);

                positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);
            }
            else if (resizerAlignment == PortAlignment.Right)
            {
                width = _originalWidth + (args.ClientX - _originalMousePosition.X);
            }
            else if (resizerAlignment == PortAlignment.Left)
            {
                width = _originalWidth - (args.ClientX - _originalMousePosition.X);

                positionX = _originalPosition.X + (args.ClientX - _originalMousePosition.X);
            }
            else if (resizerAlignment == PortAlignment.Bottom)
            {
                height = _originalHeight + (args.ClientY - _originalMousePosition.Y);
            }

            if (width < model.MinimumDimensions.Width)
            {
                width = model.MinimumDimensions.Width;
                positionX = model.Position.X;
            }
            if (height < model.MinimumDimensions.Height)
            {
                height = model.MinimumDimensions.Height;
                positionY = model.Position.Y;
            }

            model.SetPosition(positionX, positionY);
            model.Size = new Size(width, height);

            model.Refresh();
        }

        public override void Dispose()
        {
            Diagram.PointerDown -= OnPointerDown;
            Diagram.PointerMove -= OnPointerMove;
            Diagram.PointerUp -= OnPointerUp;
        }
    }
}
