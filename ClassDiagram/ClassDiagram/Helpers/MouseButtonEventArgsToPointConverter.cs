using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ClassDiagram.Helpers
{
    class MouseButtonEventArgsToPointConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (MouseButtonEventArgs) value;
            var element = (FrameworkElement) parameter;

            var point = args.GetPosition(element);
            return point;
        }
    }
}
