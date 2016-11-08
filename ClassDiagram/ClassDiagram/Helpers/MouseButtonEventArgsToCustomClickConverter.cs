using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ClassDiagram.Helpers
{
    internal class MouseButtonEventArgsToCustomClickConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (MouseButtonEventArgs) value;
            var element = (FrameworkElement) parameter;

            var point = args.GetPosition(element);
            return new CustomClickArgs(point, args);
        }
    }
}
