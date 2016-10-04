using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClassDiagram.Model;
using GalaSoft.MvvmLight;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class LineViewModel:ElementViewModel
    {
        private ILine _line;
        private bool _isConnectingBoxes;

        #region properties
        public bool IsConnectingBoxes
        {
            get { return _isConnectingBoxes; }
            set
            {
                _isConnectingBoxes = value;
                RaisePropertyChanged(propertyName: nameof(IsConnectingBoxes));
            }
        }

        public Point From
        {
            get
            {
                return new Point(0,0); // return the starting point From the lines element
            }
            set
            {
                // set the from variable inside of the line element
                RaisePropertyChanged(propertyName: nameof(From));
            }
        }

        public Point To
        {
            get
            {
                return new Point(0, 0); // return the end point to the lines element
            }
            set
            {
                // set the to property inside of the line element 
                RaisePropertyChanged(propertyName: nameof(To));
            }
        }


        #endregion

        public LineViewModel(ILine line)
        {
            _line = line;
        }

        
    }
}
