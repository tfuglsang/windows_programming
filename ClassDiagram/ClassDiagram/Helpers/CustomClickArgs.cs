using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClassDiagram.Helpers
{
    class CustomClickArgs
    {
        public CustomClickArgs(Point clickedPoint, MouseEventArgs eventArgs)
        {
            ClickedPoint = clickedPoint;
            EventArgs = eventArgs;
        }

        public Point ClickedPoint { get; }

        public MouseEventArgs EventArgs { get; }
    }
}