using System.Windows;
using System.Windows.Input;
using ClassDiagram.Model;
using GalaSoft.MvvmLight.Command;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class BoxViewModel : ElementViewModel
    {
        //private bool _isSelected;
        private Point _initialMousePostion;
        private bool _isMoving;
        private Point _initialShapePostion;


        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftBtnDown);
        public ICommand OnMouseMoveCommand => new RelayCommand<UIElement>(OnMouseMove);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);

        private void OnMouseLeftBtnDown(MouseButtonEventArgs e)
        {
            var visual = e.Source as UIElement;
            if (visual == null) return;
            //if (!IsSelected)
            //{
            //    IsSelected = true;
            //    visual.Focus();
            //    e.Handled = true;
            //    return;
            //}
            //if (!IsSelected && e.MouseDevice.Target.IsMouseCaptured) return;
            if (e.MouseDevice.Target.IsMouseCaptured) return;
            e.MouseDevice.Target.CaptureMouse();
            _initialMousePostion = Mouse.GetPosition(visual);
            _initialShapePostion = new Point(Position.X, Position.Y);
            //_canvas = VisualTreeHelper.GetParent(visual) as UIElement;
            _isMoving = true;
        }


        private void OnMouseMove(UIElement visual)
        {
            if (!_isMoving) return;

            var pos = Mouse.GetPosition(visual);
            var currentPoint = new Point(pos.X - _initialMousePostion.X, pos.Y - _initialMousePostion.Y);
            Position = currentPoint;
        }

        private void OnMouseLeftUp(MouseButtonEventArgs e)
        {
            if (!_isMoving) return;
            //UndoRedoController.AddAndExecute(new MoveShapeCommand(this, _initialShapePostion, new Point(X, Y)));
            _isMoving = false;
            Mouse.Capture(null);
            e.Handled = true;
        }

        private readonly IBox _box;

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

        public string FieldsList
        {
            get { return _box.FieldsList; }
            set
            {
                _box.FieldsList = value;
                RaisePropertyChanged();
            }
        }

        public string MethodList
        {
            get { return _box.MethodList; }
            set
            {
                _box.MethodList = value;
                RaisePropertyChanged();
            }
        }

        //public List<String> FieldsList
        //{
        //    get { return _box.FieldsList; }
        //    set
        //    {
        //        _box.FieldsList = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public List<String> MethodList
        //{
        //    get { return _box.MethodList; }
        //    set
        //    {
        //        _box.MethodList = value;
        //        RaisePropertyChanged();
        //    }
        //}

        #endregion

        public BoxViewModel(IBox box)
        {
            _box = box;

            //_box.FieldsList = new List<string>();
            //_box.MethodList = new List<string>();

            // For test purposes
            //_box.FieldsList.Add("int Alpha");
            //_box.FieldsList.Add("float Bravo");
            //_box.FieldsList.Add("double Caesar");
            //_box.MethodList.Add("int getAlpha()");
            //_box.MethodList.Add("float getBravo()");
            //_box.MethodList.Add("double getCaesar()");
            _box.FieldsList = "int Alpha";
            _box.MethodList = "float Bravo";
            _box.Label = "phoneticTranscription()";
        }
    }
}
