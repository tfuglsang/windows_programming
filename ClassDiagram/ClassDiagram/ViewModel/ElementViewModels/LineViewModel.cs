using System;
using System.ComponentModel;
using System.Windows;
using ClassDiagram.Model;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class LineViewModel : ElementViewModel
    {
        private readonly ILine _line;
        private BoxViewModel _from;
        private bool _isConnectingBoxes;
        private BoxViewModel _to;

        public LineViewModel(ILine line, BoxViewModel from, BoxViewModel to)
        {
            _line = line;
            From = from;
            To = to;
            From.PropertyChanged += BoxPropertyChanged;
            To.PropertyChanged += BoxPropertyChanged;
        }

        public LineViewModel(ILine line)
        {
            _line = line;
        }

        private void BoxPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BoxViewModel.Position))
            {
                RaisePropertyChanged(nameof(FromPoint));
                RaisePropertyChanged(nameof(ToPoint));
                RaisePropertyChanged(nameof(ArrowHeadPoints));
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
                var angleDegrees = GetAngleBetweenPoints(_from.CenterPoint, _to.CenterPoint);

                var pointToConnect = _from.CenterPoint;

                if ((45 <= angleDegrees) && (angleDegrees < 135))
                    pointToConnect.Y = pointToConnect.Y - _from.Height/2;
                else if ((135 <= angleDegrees) || (angleDegrees < -135))
                    pointToConnect.X = pointToConnect.X + _from.Width/2;
                else if ((-135 <= angleDegrees) && (angleDegrees < -45))
                    pointToConnect.Y = pointToConnect.Y + _from.Height/2;
                else if ((-45 <= angleDegrees) && (angleDegrees < 45))
                    pointToConnect.X = pointToConnect.X - _from.Width/2;

                return pointToConnect;
            }
        }

        private double GetAngleBetweenPoints(Point from, Point to)
        {
            var deltaX = from.X - to.X;
            var deltaY = from.Y - to.Y;
            var angleRadians = Math.Atan2(deltaY, deltaX);
            var angleDegrees = angleRadians*(180.0/Math.PI);

            return angleDegrees;
        }

        private double DegreesToRadians(double degrees) => degrees*(Math.PI/180);

        private Point RotatePointAroundPoint(Point pointToRotate, Point pointToRotateAround, double degrees)
        {
            // Keep degrees between 0-360 instead of -180-180
            if (degrees < 0)
                degrees = 360 + degrees;

            var returnPoint = new Point
            {
                X = pointToRotateAround.X +
                    (pointToRotate.X - pointToRotateAround.X)*Math.Cos(DegreesToRadians(degrees)) -
                    (pointToRotate.Y - pointToRotateAround.Y)*Math.Sin(DegreesToRadians(degrees)),
                Y = pointToRotateAround.Y +
                    (pointToRotate.X - pointToRotateAround.X)*Math.Sin(DegreesToRadians(degrees)) +
                    (pointToRotate.Y - pointToRotateAround.Y)*Math.Cos(DegreesToRadians(degrees))
            };

            // Rotate point X
            // Rotate point Y

            return returnPoint;
        }

        public string ArrowHeadPoints
        {
            get
            {
                var startPoint = ToPoint;
                var firstCornerPoint = RotatePointAroundPoint(new Point(startPoint.X + 10, startPoint.Y + 5), startPoint,
                    GetAngleBetweenPoints(FromPoint, ToPoint));
                var secondCornerPoint = RotatePointAroundPoint(new Point(startPoint.X + 10, startPoint.Y - 5),
                    startPoint, GetAngleBetweenPoints(FromPoint, ToPoint));
                var opositeStartPoint = RotatePointAroundPoint(new Point(startPoint.X + 20, startPoint.Y), startPoint,
                    GetAngleBetweenPoints(FromPoint, ToPoint));
                switch (Type)
                {
                    case ELine.Association:
                    case ELine.Dependency:
                        return
                            $"{firstCornerPoint.X},{firstCornerPoint.Y} {startPoint.X},{startPoint.Y} {secondCornerPoint.X},{secondCornerPoint.Y} {startPoint.X},{startPoint.Y}";
                    case ELine.Inheritance:
                    case ELine.Realization:
                        return
                            $"{startPoint.X},{startPoint.Y} {firstCornerPoint.X},{firstCornerPoint.Y} {secondCornerPoint.X},{secondCornerPoint.Y}";
                    case ELine.Aggregation:
                    case ELine.Composition:
                        return
                            $"{startPoint.X},{startPoint.Y} {firstCornerPoint.X},{firstCornerPoint.Y} {opositeStartPoint.X},{opositeStartPoint.Y} {secondCornerPoint.X},{secondCornerPoint.Y}";
                    default:
                        return "";
                }
            }
        }

        public string ArrowHeadFill
        {
            get
            {
                switch (Type)
                {
                    case ELine.Inheritance:
                    case ELine.Realization:
                    case ELine.Aggregation:
                        return "White";
                    case ELine.Composition:
                        return "Black";
                    default:
                        return "";
                }
            }
        }

        public string LineDashed
        {
            get
            {
                // Figure out which lines should be dashed
                if ((Type == ELine.Dependency) || (Type == ELine.Realization))
                    return "3,3";

                return "3,0";
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
                var angleDegrees = GetAngleBetweenPoints(_to.CenterPoint, _from.CenterPoint);

                var pointToConnect = _to.CenterPoint;

                if ((45 <= angleDegrees) && (angleDegrees < 135))
                    pointToConnect.Y = pointToConnect.Y - _to.Height/2;
                else if ((135 <= angleDegrees) || (angleDegrees < -135))
                    pointToConnect.X = pointToConnect.X + _to.Width/2;
                else if ((-135 <= angleDegrees) && (angleDegrees < -45))
                    pointToConnect.Y = pointToConnect.Y + _to.Height/2;
                else if ((-45 <= angleDegrees) && (angleDegrees < 45))
                    pointToConnect.X = pointToConnect.X - _to.Width/2;

                return pointToConnect;
            }
        }

        public int ToNumber => To.Number;

        public int FromNumber => From.Number;


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
    }
}