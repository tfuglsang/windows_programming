namespace ClassModel.Model
{
    public interface ILine
    {
        int FromNumber { get;} // The line starts at this box
        int ToNumber { get;}   // The line ends at this box
        string Label { get; }   // The name of the line shown in the View
        ELine Type { get; set; }     // Solid, dashed, dotted etc..
    }
}
