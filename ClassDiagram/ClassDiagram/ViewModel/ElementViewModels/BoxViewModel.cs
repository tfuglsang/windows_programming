using System.Collections.ObjectModel;
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

        //

        public RelayCommand AddFieldsTextBoxCommand { get; private set; }
        public RelayCommand AddMethodTextBoxCommand { get; private set; }

        public void AddFieldsTextbox()
        {
            _box.FieldsList.Add("");
            Height += 20;
        }

        public void AddMethodTextbox()
        {
            _box.MethodList.Add("");
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

        public ObservableCollection<string> FieldsList
        {
            get { return _box.FieldsList; }
            set
            {
                _box.FieldsList = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> MethodList
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

            _box.FieldsList = new ObservableCollection<string>();
            _box.MethodList = new ObservableCollection<string>();

            // For test purposes
            _box.Height = 100; 
            _box.Width = 150;
            _box.FieldsList.Add("Insert Fields1");
            //_box.FieldsList.Add("Insert Fields2");
            //_box.FieldsList.Add("Insert Fields3");
            _box.MethodList.Add("Insert Method1");
            //_box.MethodList.Add("Insert Method2");
            //_box.MethodList.Add("Insert Method3");

            _box.Label = "ClassName";

            AddFieldsTextBoxCommand = new RelayCommand(AddFieldsTextbox);
            AddMethodTextBoxCommand = new RelayCommand(AddMethodTextbox);
        }
    }
}
