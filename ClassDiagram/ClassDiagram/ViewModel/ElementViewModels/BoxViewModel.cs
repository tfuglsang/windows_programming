using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClassDiagram.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using ClassDiagram.UndoRedo.AddandRemove;

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
        private readonly IBox _box;

        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftBtnDown);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);
        public ICommand ChangeTypeCommand => new RelayCommand<string>(ChangeType);
        public RelayCommand AddFieldsTextBoxCommand { get; private set; }
        public RelayCommand AddMethodTextBoxCommand { get; private set; }
        
        public BoxViewModel(IBox box)
        {
            _box = box;

            AddFieldsTextBoxCommand = new RelayCommand(AddFieldsTextbox);
            AddMethodTextBoxCommand = new RelayCommand(AddMethodTextbox);
        }

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
            get { return new Point(_box.X + Width / 2, _box.Y + Height / 2); }
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

                var point = (Point)value;
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

        public int Number => _box.Number;
        #endregion

        private void ChangeType(string typeSelected)
        {
            Debug.Print(typeSelected);
            switch (typeSelected)
            {
                // TODO move statusbar controls to be available
                case nameof(EBox.Class):
                    UndoRedo.URController.Instance.AddExecute(new ChangeBoxType(this, EBox.Class));
                    break;
                case nameof(EBox.Abstract):
                    UndoRedo.URController.Instance.AddExecute(new ChangeBoxType(this, EBox.Abstract));
                    break;
                case nameof(EBox.Interface):
                    UndoRedo.URController.Instance.AddExecute(new ChangeBoxType(this, EBox.Interface));
                    break;
                default:
                    Debug.Print("Something went wrong - tell the user about this");
                    break;
            }
        }
        private void OnMouseLeftBtnDown(MouseButtonEventArgs e)
        {
            _wasClicked = true;
        }
        
        private void OnMouseLeftUp(MouseButtonEventArgs e)
        {
            if (_wasClicked)
                IsSelected = !IsSelected;

            _wasClicked = false;
        }

        private void AddFieldsTextbox()
        {
            _box.FieldsList.Add(new Fields(""));
            Height += 20;
        }

        private void AddMethodTextbox()
        {
            _box.MethodList.Add(new Methods(""));
            Height += 20;
        }
        
        public bool IsPointInBox(Point pointToCheck)
            =>
            (pointToCheck.X > Position.X) && (pointToCheck.X < Position.X + Width) && (pointToCheck.Y > Position.Y) &&
            (pointToCheck.Y < Position.Y + Height);
    }
}
