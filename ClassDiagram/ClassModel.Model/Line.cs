namespace ClassDiagram.Model
{
    public class Line : ILine
    {
        public int FromNumber { get; set; }
        public int ToNumber { get; set; }
        public string Label { get; set; }
        public ELine Type { get; set; }

        public Line()
        { }
    }
}
