using ClassModel.Model.Interfaces;

namespace ClassModel.Model.Implementation
{
    public class Fields : IStringHolder
    {
        public Fields(string currentString)
        {
            CurrentString = currentString;
        }
        public string CurrentString { get; set; }
    }
}