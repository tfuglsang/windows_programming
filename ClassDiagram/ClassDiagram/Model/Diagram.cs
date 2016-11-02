using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClassDiagram.Model
{
    /// <summary>
    /// Class for storing information about the lines and boxes drawn in the canvas
    /// The diagram class is used when saving this information to a .xml file, using XmlSerializer 
    /// </summary>
    public class Diagram
    {
        //[XmlIgnoreAttribute]
        //public List<IBox> Boxes { get; set; }
        //public int dummy { get; set; }
        //public string Label { get; set; }  // The name of the box shown in the View
        //public double Height { get; set; }
        //public double Width { get; set; }
        //public int Number { get; }
        //public double X { get; set; }           // Position in the canvas   
        //public double Y { get; set; }           // Position in the canvas
        //public EBox Type { get; set; }        // Square, circle, etc.
        //[XmlIgnoreAttribute]
        public List<Box> Boxes { get; set; }
        public List<Line> Lines { get; set; }

        public Diagram()
        {

        }
    }
}
