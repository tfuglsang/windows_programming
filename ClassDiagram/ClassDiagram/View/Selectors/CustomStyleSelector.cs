using System.Windows;
using System.Windows.Controls;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.View.Selectors
{
    public class CustomStyleSelector : StyleSelector
    {
        private Style _lineStyle;
        private Style _boxStyle;

        public Style LineStyle
        {
            get { return this._lineStyle; }
            set { this._lineStyle = value; }
        }

        public Style BoxStyle
        {
            get { return this._boxStyle; }
            set { this._boxStyle = value; }
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is LineViewModel)
                return LineStyle;
            if (item is BoxViewModel)
                return BoxStyle;
            return null;
        }
    }
}
