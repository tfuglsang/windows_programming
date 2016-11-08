using System.Collections.ObjectModel;

namespace ClassDiagram.Model
{
    public interface IBox
    {
        ObservableCollection<Fields> FieldsList { get; set; }   // List of fields in the box
        ObservableCollection<Methods> MethodList { get; set; }   // List of methods in the box
        string Label { get; set; }  // The name of the box shown in the View
        double Height { get; set; } 
        double Width { get; set; }
        int Number { get; }         
        double X { get; set;  }           // Position in the canvas   
        double Y { get; set; }           // Position in the canvas
        EBox Type { get; set; }        // Square, circle, etc.
    }
}
