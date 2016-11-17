using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace ClassDiagram.Model
{
    public class Box: IBox
    {
        public Box()
        {
            Height = 100;
            Width = 150;
            X = 300;
            Y = 300;
            FieldsList = new ObservableCollection<Fields>();
            MethodList = new ObservableCollection<Methods>();
        }

        public string Label { get; set; }          // The name of the box shown in the View
        [XmlIgnore]
        public ObservableCollection<Fields> FieldsList { get; set; }          
        public List<string> FieldsStringList { get; set; }
        [XmlIgnore]
        public ObservableCollection<Methods> MethodList { get; set; }
        public List<string> MethodStringList { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int Number { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        //[XmlIgnore]
        public EBox Type { get; set; }
        //public List<String> FieldsList { get; set; }
        //public List<String> MethodList { get; set; }
    }
}
