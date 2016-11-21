using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassDiagram.CopyPaste;
using ClassDiagram.ViewModel.ElementViewModels;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClassDiagram.View.UserControls
{
    /// <summary>
    /// Interaction logic for BoxUserControl.xaml
    /// </summary>
    public partial class BoxUserControl : UserControl
    {
        public BoxUserControl()
        {
            InitializeComponent();
            // Make it possible to bind to this usercontrol from the context menu by setting the namescope to search for elementnames in.
            NameScope.SetNameScope(ContextMenu, NameScope.GetNameScope(this));
        }

        public static readonly RoutedEvent CutEvent = EventManager.RegisterRoutedEvent(
            "CutEventHandler",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(BoxUserControl));

        public event RoutedEventHandler CutEventHandler
        {
            add { AddHandler(CutEvent, value); }
            remove { RemoveHandler(CutEvent, value); }
        }

        public ICommand CutCommand => new RelayCommand<Guid>(Cut);
        public ICommand CopyCommand => new RelayCommand<Guid>(Copy);

        private void Copy(Guid commandParam)
        {
            var newEventArgs = new CopyPasteArgs(CutEvent, commandParam, CopyPasteArgs.COPY);
            RaiseEvent(newEventArgs);
        }
        private void Cut(Guid commandParam)
        {
            Debug.Print($"{commandParam}");
            var newEventArgs = new CopyPasteArgs(CutEvent, commandParam, CopyPasteArgs.CUT);
            RaiseEvent(newEventArgs);
        }
    }
}
