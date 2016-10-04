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

        public int toNumber
        {
            get { return _line.ToNumber; }
        }

        public int fromNumber { get { return _line.FromNumber; } }


        public ELine Type
        {
            get { return _line.Type; }
            set
            {
                _line.Type = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public LineViewModel(ILine line)
        {
            _line = line;
        }

        
    }
}
