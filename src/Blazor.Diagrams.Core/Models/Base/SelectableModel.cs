using System;

namespace Blazor.Diagrams.Core.Models.Base
{
    public abstract class SelectableModel : Model
    {
        private int _order;
        private bool _selected;

        public event Action<SelectableModel>? OrderChanged;
        public event Action<SelectableModel>? SelectionChanged;

        protected SelectableModel() { }

        protected SelectableModel(string id) : base(id) { }

        public bool Selected 
        { 
            get => _selected;
            internal set
            {
                if (value == Selected)
                    return;

                _selected = value;
                SelectionChanged?.Invoke(this);
            }
        }

        public int Order
        {
            get => _order;
            set
            {
                if (value == Order)
                    return;

                _order = value;
                OrderChanged?.Invoke(this);
            }
        }
    }
}
