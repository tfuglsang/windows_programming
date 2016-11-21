using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClassDiagram.CopyPaste
{
    public class CopyPasteArgs : RoutedEventArgs
    {
        public const int COPY = 1;
        public const int CUT = 2;
        public CopyPasteArgs()
        {
        }

        public CopyPasteArgs(Guid boxId) : base()
        {
            BoxId = boxId;
        }

        public CopyPasteArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }

        public CopyPasteArgs(RoutedEvent routedEvent, Guid boxId, int type) : base(routedEvent)
        {
            BoxId = boxId;
            Type = type;
        }

        public CopyPasteArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }

        public Guid BoxId { get; set; }
        public int Type { get; set; }
    }
}
