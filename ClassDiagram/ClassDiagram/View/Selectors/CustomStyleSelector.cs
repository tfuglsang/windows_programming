using System.Windows;
using System.Windows.Controls;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.View.Selectors
{
    public class CustomStyleSelector : StyleSelector
    {
        public Style LineStyle { get; set; }

        public Style BoxStyle { get; set; }

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
