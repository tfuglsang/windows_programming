using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using System.Windows;
using GalaSoft.MvvmLight;

namespace ClassDiagram.ViewModel
{
    class StatusBarViewModel : ViewModelBase
    {
        private Point _statusBarCoordinates;
        private string _statusBarMessage;

        private StatusBarViewModel() { }

        public static StatusBarViewModel Instance { get; } = new StatusBarViewModel();

        public Point StatusBarCoordinates
        {
            get { return _statusBarCoordinates; }
            set { Set(ref _statusBarCoordinates, value); }
        }

        public string StatusBarMessage
        {
            get { return _statusBarMessage; }
            set { Set(ref _statusBarMessage, value); }
        }
    }
}
