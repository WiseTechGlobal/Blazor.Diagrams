using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Positions.Resizing
{
    public class TopRightResizerProvider : IResizeProvider
    {
        private Size _originalSize = null!;
        private Point _originalPosition = null!;
        private Point _originalMousePosition = null!;
        private NodeModel _nodeModel = null!;
        private Diagram _diagram = null!;

        public Point? GetPosition(Model model)
        {
            if (model is NodeModel nodeModel && nodeModel.Size is not null)
            {
                return new Point(nodeModel.Position.X + nodeModel.Size.Width + 5, nodeModel.Position.Y + nodeModel.Size.Height + 5);
            }
            return null;
        }

        public void OnResizeStart(Diagram diagram, Model model, PointerEventArgs eventArgs)
        {
            if (model is NodeModel nodeModel)
            {
                _originalPosition = new Point(nodeModel.Position.X, nodeModel.Position.Y);
                _originalMousePosition = new Point(eventArgs.ClientX, eventArgs.ClientY);
                _originalSize = nodeModel.Size!;
                _nodeModel = nodeModel;
                _diagram = diagram;
            }
        }

        public void OnPointerMove(Model? model, PointerEventArgs args)
        {
            if (_nodeModel is null)
            {
                return;
            }
            var width = _originalSize.Width;
            var height = _originalSize.Height;
            var positionX = _nodeModel.Position.X;
            var positionY = _nodeModel.Position.Y;

            height = _originalSize.Height - (args.ClientY - _originalMousePosition.Y);
            width = _originalSize.Width + (args.ClientX - _originalMousePosition.X);

            positionX = _originalPosition.X;
            positionY = _originalPosition.Y + (args.ClientY - _originalMousePosition.Y);

            //if (width < model.MinimumDimensions.Width)
            //{
            //    width = model.MinimumDimensions.Width;
            //    positionX = model.Position.X;
            //}
            //if (height < model.MinimumDimensions.Height)
            //{
            //    height = model.MinimumDimensions.Height;
            //    positionY = model.Position.Y;
            //}
            _nodeModel.SetPosition(positionX, positionY);
            _nodeModel.Size = new Size(width, height);
        }

        public void OnResizeEnd(Model? model, PointerEventArgs args)
        {
            _originalSize = null!;
            _originalPosition = null!;
            _originalMousePosition = null!;
            _nodeModel = null!;
            _diagram = null!;
        }

    }
}
