﻿using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.Diagrams.Core.Models
{
    public class GroupModel : NodeModel
    {
        private readonly List<NodeModel> _children;

        public GroupModel(IEnumerable<NodeModel> children, byte padding = 30, bool autoSize = true)
        {
            _children = new List<NodeModel>();

            Size = Size.Zero;
            Padding = padding;
            AutoSize = autoSize;
            Initialize(children);
        }

        public IReadOnlyList<NodeModel> Children => _children;
        public byte Padding { get; }
        public bool AutoSize { get; }

        public void AddChild(NodeModel child)
        {
            _children.Add(child);
            child.Group = this;
            child.SizeChanged += OnNodeChanged;
            child.Moving += OnNodeChanged;

            if (UpdateDimensions())
            {
                Refresh();
            }
        }

        public void RemoveChild(NodeModel child)
        {
            if (!_children.Remove(child))
                return;

            child.Group = null;
            child.SizeChanged -= OnNodeChanged;
            child.Moving -= OnNodeChanged;

            if (UpdateDimensions())
            {
                Refresh();
                RefreshLinks();
            }
        }

        public override void SetPosition(double x, double y)
        {
            Console.WriteLine($"({(Group == null ? "Parent" : "Child")}) SetPosition {x:00} {y:00}");
            var deltaX = x - Position.X;
            var deltaY = y - Position.Y;
            base.SetPosition(x, y);

            foreach (var node in Children)
                node.UpdatePositionSilently(deltaX, deltaY);

            Refresh();
            RefreshLinks();
        }

        public override void UpdatePositionSilently(double deltaX, double deltaY)
        {
            Console.WriteLine($"({(Group == null ? "Parent" : "Child")}) UpdatePositionSilently {deltaX:00} {deltaY:00}");
            base.UpdatePositionSilently(deltaX, deltaY);

            foreach (var child in Children)
                child.UpdatePositionSilently(deltaX, deltaY);

            Refresh();
        }

        public void Ungroup()
        {
            foreach (var child in Children)
            {
                child.Group = null;
                child.SizeChanged -= OnNodeChanged;
                child.Moving -= OnNodeChanged;
            }

            _children.Clear();
        }

        public override ResizerModel AddResizer(PortAlignment alignment)
            => throw new NotImplementedException();

        public override ResizerModel AddResizer(ResizerModel model) => throw new NotImplementedException();

        public override bool RemoveResizer(ResizerModel model) => throw new NotImplementedException();

        private void Initialize(IEnumerable<NodeModel> children)
        {
            foreach (var child in children)
            {
                _children.Add(child);
                child.Group = this; 
                child.SizeChanged += OnNodeChanged;
                child.Moving += OnNodeChanged;
            }

            UpdateDimensions();
        }

        private void OnNodeChanged(NodeModel node)
        {
            if (UpdateDimensions())
            {
                Refresh();
            }
        }

        private bool UpdateDimensions()
        {
            if (Children.Count == 0)
                return true;

            if (Children.Any(n => n.Size == null))
                return false;

            var bounds = Children.GetBounds();

            var newPosition = new Point(bounds.Left - Padding, bounds.Top - Padding);
            if (!Position.Equals(newPosition))
            {
                Position = newPosition;
                TriggerMoving();
            }

            Size = new Size(bounds.Width + Padding * 2, bounds.Height + Padding * 2);
            return true;
        }
    }
}
