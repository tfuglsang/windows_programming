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
        private BoxViewModel _from;
        private BoxViewModel _to;

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

        public BoxViewModel From
        {
            get { return _from; }
            set { Set(ref _from, value); }
        }

        public BoxViewModel To
        {
            get { return _to; }
            set { Set(ref _to, value); }
        }

        public int ToNumber => _line.ToNumber;

        public int FromNumber => _line.FromNumber;


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
