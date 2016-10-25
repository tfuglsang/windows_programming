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
    public class BoxViewModel : ElementViewModel
    {
        private IBox _box;
        private Box Box { get; }
        public int Number => Box.Number;

        #region properties

        public double Height
        {
            get { return _box.Height; }
            set
            {
                _box.Height = value;
                RaisePropertyChanged();
            }
        }

        public double Width
        {
            get { return _box.Width; }
            set
            {
                _box.Width = value;
                RaisePropertyChanged();
            }
        }

        public Point Position
        {
            get { return new Point(_box.X, _box.Y); 
            }
            set
            {
                _box.X = value.X;
                _box.Y = value.Y;
                RaisePropertyChanged();
            }
        }

        public EBox Type
        {
            get { return _box.Type; }
            set
            {
                _box.Type = value;
                RaisePropertyChanged();
            }
        }

        public string Label
        {
            get { return _box.Label; }
            set
            {
                _box.Label = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public BoxViewModel(IBox box)
        {
            _box = box;
        }
    }
}
