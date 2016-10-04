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
        private Ibox _box;

        #region properties
        public int Height {
            get
            {
                // TDOD implement this to return a value from the box
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
                // this has to return the width from the box element
            }
            set
            {
                // this need to set the width from the box element
                RaisePropertyChanged(propertyName: nameof(Width));
            }
        }
        public Point Position
        {
            get
            {
                return new Point(0,0); // return the position from the box object
            }
            set
            {
                // set the position inside of the box object
                RaisePropertyChanged(propertyName: nameof(Position));
            }
        }
        #endregion 

        public BoxViewModel(Ibox box)
        {
            _box = box;
        }
    }
}
