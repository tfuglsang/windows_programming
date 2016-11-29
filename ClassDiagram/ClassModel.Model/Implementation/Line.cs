using System;
using ClassModel.Model.Interfaces;

namespace ClassModel.Model.Implementation
{
    public class Line : ILine
    {
        public Guid FromBoxId { get; set; }
        public Guid ToBoxId { get; set; }
        public string Label { get; set; }
        public ELine Type { get; set; }

        public Line()
        { }
    }
}
