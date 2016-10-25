using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClassDiagram.Model
{
    public class Box: IBox
    {
        public Box()
        {
            Height = 100;
            Width = 100;
            X = 300;
            Y = 300;
        }
        public RelayCommand addTextBoxCommand { get; set; }
        public string Label { get; set; }          // The name of the box shown in the View
        public ObservableCollection<String> FieldsList { get; set; }          // The name of the box shown in the View
        public ObservableCollection<String> MethodList { get; set; }          // The name of the box shown in the View
        public double Height { get; set; }
        public double Width { get; set; }
        public int Number { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public EBox Type { get; set; }
        //public List<String> FieldsList { get; set; }
        //public List<String> MethodList { get; set; }
    }
}
