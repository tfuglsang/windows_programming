﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.Model
{
    public interface IBox
    {
        string Label { get; set; }  // The name of the box shown in the View
        double Height { get; set; } 
        double Width { get; set; }
        int Number { get; }         
        double X { get; set;  }           // Position in the canvas   
        double Y { get; set; }           // Position in the canvas
        EBox Type { get; set; }        // Square, circle, etc.
    }
}