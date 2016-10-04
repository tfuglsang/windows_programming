using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.Model
{
    public interface ILine
    {
        int FromNumber { get; } // The line starts at this box
        int ToNumber { get; }   // The line ends at this box
        string Label { get; }   // The name of the line shown in the View
        ELine Type { get; }     // Solid, dashed, dotted etc..
    }
}
