using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ClassDiagram.Model;
using ClassDiagram.UndoRedo.AddandRemove;
using GalaSoft.MvvmLight.Command;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class LineViewModel : ElementViewModel
    {
        private readonly ILine _line;
        public ILine Line => _line;
        private bool _isConnectingBoxes;

        public ICommand SelectLineCommand => new RelayCommand<MouseButtonEventArgs>(SelectLine);
        public ICommand ChangeTypeCommand => new RelayCommand<string>(ChangeType);
        
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
        private void ChangeType(string selectedType)
        {
            Debug.Print(selectedType);
            switch (selectedType)
            {
                case nameof(ELine.Association):
                    UndoRedo.URController.Instance.AddExecute(new ChangeLineType(this, ELine.Association));
                    break;
                case nameof(ELine.Dependency):
                    UndoRedo.URController.Instance.AddExecute(new ChangeLineType(this, ELine.Dependency));
                    break;
                case nameof(ELine.Aggregation):
                    UndoRedo.URController.Instance.AddExecute(new ChangeLineType(this, ELine.Aggregation));
                    break;
                case nameof(ELine.Composition):
                    UndoRedo.URController.Instance.AddExecute(new ChangeLineType(this, ELine.Composition));
                    break;
                case nameof(ELine.Inheritance):
                    UndoRedo.URController.Instance.AddExecute(new ChangeLineType(this, ELine.Inheritance));
                    break;
                case nameof(ELine.Realization):
                    UndoRedo.URController.Instance.AddExecute(new ChangeLineType(this, ELine.Realization));
                    break;
                default:
                    Debug.Print("Something went wrong - The user should be told");
                    break;
            }
        }
        private void SelectLine(MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            e.Handled = true;
        }
        private void BoxPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BoxViewModel.Position) || e.PropertyName == nameof(BoxViewModel.Height))
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

        private BoxViewModel From { get; }

        public Point FromPoint
        {
            get
            {
                var angleDegrees = GetAngleBetweenPoints(From.CenterPoint, To.CenterPoint);

                var pointToConnect = From.CenterPoint;

                if ((45 <= angleDegrees) && (angleDegrees < 135))
                    pointToConnect.Y = pointToConnect.Y - From.Height/2;
                else if ((135 <= angleDegrees) || (angleDegrees < -135))
                    pointToConnect.X = pointToConnect.X + From.Width/2;
                else if ((-135 <= angleDegrees) && (angleDegrees < -45))
                    pointToConnect.Y = pointToConnect.Y + From.Height/2;
                else if ((-45 <= angleDegrees) && (angleDegrees < 45))
                    pointToConnect.X = pointToConnect.X - From.Width/2;

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
                        return "Transparent";
                }
            }
        }

        public string LineDashed
        {
            get
            {
                if ((Type == ELine.Dependency) || (Type == ELine.Realization))
                    return "3,3";

                return "3,0";
            }
        }

        private BoxViewModel To { get; }

        public Point ToPoint
        {
            get
            {
                var angleDegrees = GetAngleBetweenPoints(To.CenterPoint, From.CenterPoint);

                var pointToConnect = To.CenterPoint;

                if ((45 <= angleDegrees) && (angleDegrees < 135))
                    pointToConnect.Y = pointToConnect.Y - To.Height/2;
                else if ((135 <= angleDegrees) || (angleDegrees < -135))
                    pointToConnect.X = pointToConnect.X + To.Width/2;
                else if ((-135 <= angleDegrees) && (angleDegrees < -45))
                    pointToConnect.Y = pointToConnect.Y + To.Height/2;
                else if ((-45 <= angleDegrees) && (angleDegrees < 45))
                    pointToConnect.X = pointToConnect.X - To.Width/2;

                return pointToConnect;
            }
        }

        public Guid ToBoxId => _line.ToBoxId;

        public Guid FromBoxId => _line.FromBoxId;


        public ELine Type
        {
            get { return _line.Type; }
            set
            {
                _line.Type = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ArrowHeadPoints));
                RaisePropertyChanged(nameof(ArrowHeadFill));
            }
        }

        #endregion
    }
}