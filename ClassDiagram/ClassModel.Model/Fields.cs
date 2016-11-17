using System;
using System.Collections.ObjectModel;

namespace ClassDiagram.Model
{
    public class Fields
    {
        public Fields(string aField)
        {
            Field = aField;
        }
        public string Field { get; set; }
    }
}