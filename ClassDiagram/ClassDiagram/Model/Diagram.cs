using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.Model
{
    /// <summary>
    /// Class for storing information about the lines and boxes drawn in the canvas
    /// The diagram class is used when saving this information to a .xml file, using XmlSerializer 
    /// </summary>
    class Diagram
    {
        public List<Box> Boxes { get; set; }
        public List<Line> Lines { get; set; }
    }
}
