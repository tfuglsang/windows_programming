using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ClassDiagram.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using ClassDiagram.UndoRedo.AddandRemove;
using System.Windows.Controls;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class BoxViewModel : ElementViewModel
    {
        private bool _wasClicked;
        private Point? _startingOffset = null;
        private Point? _startingPosition = null;
        private readonly IBox _box;

        public ICommand OnMouseLeftBtnDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftBtnDown);
        public ICommand OnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseLeftUp);
        public ICommand ChangeTypeCommand => new RelayCommand<string>(ChangeType);
        public ICommand CopyCommand => new RelayCommand(Copy);

        private void Copy()
        {
            CopyPaste.CopyPasteController.Instance.Copy(this);
        }
        public RelayCommand AddFieldsTextBoxCommand { get; private set; }
        public RelayCommand AddMethodTextBoxCommand { get; private set; }
        
        public RelayCommand TextChangedCommand { get; private set; }        
        private Box _oldbox; // Used for undo/redo of text         

        private UndoRedo.URController UndoRedo { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public BoxViewModel(IBox box)
        {
            _box = box;
            

            AddFieldsTextBoxCommand = new RelayCommand(AddFieldsTextbox);
            AddMethodTextBoxCommand = new RelayCommand(AddMethodTextbox);
            TextChangedCommand = new RelayCommand(TextChanged);

            UndoRedo = ClassDiagram.UndoRedo.URController.Instance;
            UndoCommand = UndoRedo.UndoC;
            RedoCommand = UndoRedo.RedoC;

            _oldbox = new Box();

            string s = this.Label;
            _oldbox.Label = s;
            foreach (Fields f in this.FieldsList)
            {
                s = f.Field;
                Fields field = new Fields(s);
                _oldbox.FieldsList.Add(field);
            }
            foreach (Methods f in this.MethodList)
            {
                s = f.Method;
                Methods method = new Methods(s);
                _oldbox.MethodList.Add(method);
            }

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
            get {
                return _box.FieldsList;
            }
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
                    Type = EBox.Class;
                    break;
                case nameof(EBox.Abstract):
                    Type = EBox.Abstract;
                    break;
                case nameof(EBox.Interface):
                    Type = EBox.Interface;
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

        private void TextChanged()
        {
            // Check if text boxes are the same as before a keystroke 
            string s1;
            string s2;

            bool label_same = true;

            if (this.Label == _oldbox.Label)
            {
                label_same = true;
            }
            else
            {
                label_same = false;
            }

            bool fields_same = true;

            if (FieldsList.Count == _oldbox.FieldsList.Count)
            {
                for (int i = 0; i < FieldsList.Count; i++)
                {
                    s1 = FieldsList[i].Field;
                    s2 = _oldbox.FieldsList[i].Field;
                    if (s1 == s2)
                    {

                        fields_same = true;
                    }
                    else
                    {
                        fields_same = false;
                        break;
                    }
                }
            }
            else
            {
                fields_same = false;
            }

            bool methods_same = true;
            if (MethodList.Count == _oldbox.MethodList.Count)
            {
                for (int i = 0; i < MethodList.Count; i++)
                {
                    s1 = MethodList[i].Method;
                    s2 = _oldbox.MethodList[i].Method;

                    if (s1 == s2)
                    {
                        methods_same = true;
                    }
                    else
                    {
                        methods_same = false;
                        break;
                    }
                }
            }
            else
            {
                methods_same = false;
            }

            // If the text has changed, update the undo/redo controller
            if (methods_same == false || fields_same == false || label_same == false)
            {
                UndoRedo.AddExecute(new TextChanged(this, _oldbox, _box));

                // Then update the 'old' textbox to reflect the newest text
                string s;
                int i = 0;
                foreach (Fields f in this.FieldsList)
                {
                    s = f.Field;
                    Fields field = new Fields(s);

                    if (_oldbox.FieldsList.Count - 1 < i)
                    {
                        _oldbox.FieldsList.Add(new Fields(""));
                        _oldbox.FieldsList[i] = field;
                    }
                    else
                    {
                        _oldbox.FieldsList[i] = field;
                    }
                    i++;
                }

                i = 0;
                foreach (Methods f in this.MethodList)
                {
                    s = f.Method;
                    Methods method = new Methods(s);

                    if (_oldbox.MethodList.Count - 1 < i)
                    {
                        _oldbox.MethodList.Add(new Methods(""));
                        _oldbox.MethodList[i] = method;
                    }
                    else
                    {
                        _oldbox.MethodList[i] = method;
                    }
                    i++;
                }
                s = Label;
                _oldbox.Label = s;
            }
        }

        public bool IsPointInBox(Point pointToCheck)
            =>
            (pointToCheck.X > Position.X) && (pointToCheck.X < Position.X + Width) && (pointToCheck.Y > Position.Y) &&
            (pointToCheck.Y < Position.Y + Height);
    }

}
