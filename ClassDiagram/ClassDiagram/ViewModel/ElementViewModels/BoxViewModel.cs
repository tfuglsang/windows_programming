using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using ClassDiagram.Model; 

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class BoxViewModel: ElementViewModel
    {
        private IBox _box;

        #region properties
        public int Height {
            get
            {
                // TDOD implement this to return a value From the box
                return 0;
            }
            set
            {
                // Set the height value inside the box object
                RaisePropertyChanged(propertyName: nameof(Height));
            }
        }
        public int Width
        {
            get
            {
                return 0;
                // this has to return the width From the box element
            }
            set
            {
                // this need to set the width From the box element
                RaisePropertyChanged(propertyName: nameof(Width));
            }
        }
        public Point Position
        {
            get
            {
                return new Point(0,0); // return the position From the box object
            }
            set
            {
                // set the position inside of the box object
                RaisePropertyChanged(propertyName: nameof(Position));
            }
        }
        #endregion 

        public BoxViewModel(IBox box)
        {
            _box = box;
        }
    }
}
