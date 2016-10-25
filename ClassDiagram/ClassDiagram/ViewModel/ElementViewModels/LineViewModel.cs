using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using ClassDiagram.Model;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class LineViewModel : ElementViewModel
    {
        private readonly ILine _line;
        private bool _isConnectingBoxes;
        private BoxViewModel _from;
        private BoxViewModel _to;

        public LineViewModel(ILine line, BoxViewModel from, BoxViewModel to)
        {
            _line = line;
            From = from;
            To = to;
            From.PropertyChanged += BoxPropertyChanged;
            To.PropertyChanged += BoxPropertyChanged;
        }

        private void BoxPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                RaisePropertyChanged("FromPoint");
                RaisePropertyChanged("ToPoint");
            }
        }

        #region properties

        public bool IsConnectingBoxes
        {
            get { return _isConnectingBoxes; }
            set
            {
                _isConnectingBoxes = value;
                RaisePropertyChanged(nameof(IsConnectingBoxes));
            }
        }

        public BoxViewModel From
        {
            get { return _from; }
            set { Set(ref _from, value); }
        }

        public Point FromPoint
        {
            get
            {
                // This should not run on Position but rather on center position
                var deltaX = _from.Position.X - _to.Position.X;
                var deltaY = _from.Position.X - _to.Position.Y;
                var angle = Math.Atan2(deltaY, deltaX);
                if ((0 < angle) && (angle < Math.PI/2))
                    Debug.Print("First Quadrant");
                else if ((Math.PI/2 < angle) && (angle < Math.PI))
                    Debug.Print("Second Quadrant");
                else if ((-Math.PI < angle) && (angle < -Math.PI/2))
                    Debug.Print("Third Quadrant");
                else if ((-Math.PI/2 < angle) && (angle < 0))
                    Debug.Print("Fourth Quadrant");
                return _from.Position;
            }
        }

        public BoxViewModel To
        {
            get { return _to; }
            set { Set(ref _to, value); }
        }

        public Point ToPoint
        {
            get { return _to.Position; }
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
