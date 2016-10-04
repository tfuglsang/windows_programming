﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.Model
{
    class Box
    {
        string Label { get; set; }          // The name of the box shown in the View
        public double Height { get; set; }
        public double Width { get; set; }
        public int Number { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public EBox Type { get; set; }
    }
}