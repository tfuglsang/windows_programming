using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.Model
{
    public class Coordinates
    {
        public Coordinates(int x, int y)
        {
            Coordinate_x = x;
            Coordinate_y = y;
        }
        public int Coordinate_x { get; set; }
        public int Coordinate_y { get; set; }
    }
}
