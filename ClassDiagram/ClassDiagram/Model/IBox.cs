using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClassDiagram.Model
{
    public interface IBox
    {
        RelayCommand addTextBoxCommand { get; set; }
        List<String> FieldsList { get; set; }   // List of fields in the box
        List<String> MethodList { get; set; }   // List of methods in the box
        string Label { get; set; }  // The name of the box shown in the View
        double Height { get; set; } 
        double Width { get; set; }
        int Number { get; }         
        double X { get; set;  }           // Position in the canvas   
        double Y { get; set; }           // Position in the canvas
        EBox Type { get; set; }        // Square, circle, etc.
    }
}
