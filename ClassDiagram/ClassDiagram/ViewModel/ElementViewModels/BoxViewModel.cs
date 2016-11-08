using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClassModel.Model;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class BoxViewModel : ElementViewModel
    {
        //private bool _isSelected;
        private Point _initialMousePostion;
        private bool _isMoving;
        private bool _hasMoved;
        private bool _wasClicked;
        private Point? _startingOffset = null;
        private Point? _startingPosition = null;

        public Point? StartingOffset
        {
            get { return _startingOffset; }
            set
            {
                if (value == null)
                {
                    _startingOffset = null;
                    return;
                }

                var point = (Point) value;
                _startingOffset = new Point(point.X - Position.X, point.Y - Position.Y);
                Debug.Print($"{_startingOffset.Value.X},{_startingOffset.Value.Y}");
            }
        }

        public Point? StartingPosition
        {
            get { return _startingOffset; }
            set
            {
                if (value == null)
                {
                    _startingOffset = null;
                    return;
                }

                _startingPosition = value.Value;
            }
        }

        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftBtnDown);
        public ICommand OnMouseMoveCommand => new RelayCommand<UIElement>(OnMouseMove);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);

        private void OnMouseLeftBtnDown(MouseButtonEventArgs e)
        {
            _wasClicked = true;
        }

        /// <summary>
        /// THIS METHOD IS UNUSED
        /// </summary>
        /// <param name="visual"></param>
        private void OnMouseMove(UIElement visual)
        {
            // THIS METHOD IS UNUSED

            if (!_isMoving) return;

            var pos = Mouse.GetPosition(visual);
            var currentPoint = new Point(pos.X - _initialMousePostion.X, pos.Y - _initialMousePostion.Y);

            // Ensure that box is within the canvas limits
            Size grid_size = visual.RenderSize;            
            
            if (currentPoint.X > grid_size.Width - Width)
            {
                currentPoint.X = grid_size.Width - Width;
            }
            if (currentPoint.Y > grid_size.Height - Height)
            {
                currentPoint.Y = grid_size.Height - Height;
            }
            if (currentPoint.X < 0 )
            {
                currentPoint.X = 0 ;
            }
            if (currentPoint.Y < 0 )
            {
                currentPoint.Y = 0 ;
            }

            Position = currentPoint;
            _hasMoved = true;
        }

        private void OnMouseLeftUp(MouseButtonEventArgs e)
        {
            if (_wasClicked)
                IsSelected = true;

            _wasClicked = false;
        }

        private readonly IBox _box;

        //

        public RelayCommand AddFieldsTextBoxCommand { get; private set; }
        public RelayCommand AddMethodTextBoxCommand { get; private set; }

        public void AddFieldsTextbox()
        {
            _box.FieldsList.Add(new Fields(""));
            Height += 20;
        }

        public void AddMethodTextbox()
        {
            _box.MethodList.Add(new Methods(""));
            Height += 20;
        }
        
        public int Number => _box.Number;
        
        public bool IsPointInBox(Point pointToCheck)
            =>
            (pointToCheck.X > Position.X) && (pointToCheck.X < Position.X + Width) && (pointToCheck.Y > Position.Y) &&
            (pointToCheck.Y < Position.Y + Height);

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
            get { return new Point(_box.X, _box.Y); }
            set
            {
                _box.X = value.X;
                _box.Y = value.Y;
                RaisePropertyChanged();
            }
        }

        public Point CenterPoint
        {
            get { return new Point(_box.X + Width/2, _box.Y + Height/2); }
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

        public ObservableCollection<Fields> FieldsList
        {
            get { return _box.FieldsList; }
            set
            {
                _box.FieldsList = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Methods> MethodList
        {
            get { return _box.MethodList; }
            set
            {
                _box.MethodList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public BoxViewModel(IBox box)
        {
            _box = box;

            _box.FieldsList = new ObservableCollection<Fields>();
            _box.MethodList = new ObservableCollection<Methods>();

            // For test purposes
            _box.Height = 100; 
            _box.Width = 150;
            _box.FieldsList.Add(new Fields("Insert Fields1"));
            //_box.FieldsList.Add("Insert Fields2");
            //_box.FieldsList.Add("Insert Fields3");
            _box.MethodList.Add(new Methods("Insert Method1"));
            //_box.MethodList.Add("Insert Method2");
            //_box.MethodList.Add("Insert Method3");

            _box.Label = "ClassName";

            AddFieldsTextBoxCommand = new RelayCommand(AddFieldsTextbox);
            AddMethodTextBoxCommand = new RelayCommand(AddMethodTextbox);
        }
    }
}
