using System;
using System.ComponentModel;
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
                var deltaX = _from.CenterPoint.X - _to.CenterPoint.X;
                var deltaY = _from.CenterPoint.X - _to.CenterPoint.Y;
                var angleRadians = Math.Atan2(deltaY, deltaX);
                var angleDegrees = angleRadians*(180.0/Math.PI);

                var pointToConnect = _from.CenterPoint;

                if ((45 < angleDegrees) && (angleDegrees < 135))
                    pointToConnect.Y = pointToConnect.Y - _from.Height/2;
                else if ((135 < angleDegrees) || (angleDegrees < -135))
                    pointToConnect.X = pointToConnect.X + _from.Width/2;
                else if ((-135 < angleDegrees) && (angleDegrees < -45))
                    pointToConnect.Y = pointToConnect.Y + _from.Height/2;
                else if ((-45 < angleDegrees) && (angleDegrees < 45))
                    pointToConnect.X = pointToConnect.X - _from.Width/2;

                return pointToConnect;
            }
        }

        public BoxViewModel To
        {
            get { return _to; }
            set { Set(ref _to, value); }
        }

        public Point ToPoint
        {
            get
            {
                var deltaX = _to.CenterPoint.X - _from.CenterPoint.X;
                var deltaY = _to.CenterPoint.X - _from.CenterPoint.Y;
                var angleRadians = Math.Atan2(deltaY, deltaX);
                var angleDegrees = angleRadians*(180.0/Math.PI);

                var pointToConnect = _to.CenterPoint;

                if ((45 < angleDegrees) && (angleDegrees < 135))
                    pointToConnect.Y = pointToConnect.Y - _to.Height/2;
                else if ((135 < angleDegrees) || (angleDegrees < -135))
                    pointToConnect.X = pointToConnect.X + _to.Width/2;
                else if ((-135 < angleDegrees) && (angleDegrees < -45))
                    pointToConnect.Y = pointToConnect.Y + _to.Height/2;
                else if ((-45 < angleDegrees) && (angleDegrees < 45))
                    pointToConnect.X = pointToConnect.X - _to.Width/2;

                return pointToConnect;
            }
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
