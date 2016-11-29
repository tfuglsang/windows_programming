using System;

namespace ClassModel.Model.Interfaces
{
    public interface ILine
    {
        Guid FromBoxId { get;} // The line starts at this box
        Guid ToBoxId { get;}   // The line ends at this box
        string Label { get; }   // The name of the line shown in the View
        ELine Type { get; set; }     // Solid, dashed, dotted etc..
    }
}
