using System;
using ClassModel.Model.Interfaces;

namespace ClassModel.Model.Implementation
{
    public class Methods : IStringHolder
    {
        public Methods(string currentString)
        {
            CurrentString = currentString;
        }
        public string CurrentString { get; set; }
    }
}