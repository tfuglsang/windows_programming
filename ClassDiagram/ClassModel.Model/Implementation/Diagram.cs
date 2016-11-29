using System.Collections.Generic;

namespace ClassModel.Model.Implementation
{
    /// <summary>
    /// Class for storing information about the lines and boxes drawn in the canvas
    /// The diagram class is used when saving this information to a .xml file, using XmlSerializer 
    /// </summary>
    public class Diagram
    {
        public List<Box> Boxes { get; set; }
        public List<Line> Lines { get; set; }

        public Diagram()
        {

        }
    }
}
