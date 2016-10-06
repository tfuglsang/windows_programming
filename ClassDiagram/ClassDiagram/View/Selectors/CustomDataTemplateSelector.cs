using System.Windows;
using System.Windows.Controls;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.View.Selectors
{
    public class CustomDataTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _boxTemplate;
        private DataTemplate _lineTemplate;

        public DataTemplate BoxTemplate
        {
            get { return _boxTemplate; }
            set { _boxTemplate = value; }
        }

        public DataTemplate LineTemplate
        {
            get { return _lineTemplate; }
            set { _lineTemplate = value; }
        }

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
