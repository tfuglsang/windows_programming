using System.Windows;
using System.Windows.Controls;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.View.Selectors
{
    public class CustomDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoxTemplate { get; set; }

        public DataTemplate LineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BoxViewModel)
                return BoxTemplate;
            if (item is LineViewModel)
                return LineTemplate;
            return null;
        }
    }
}
