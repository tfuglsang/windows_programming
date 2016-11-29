using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ClassDiagram.Helpers;
using ClassDiagram.ViewModel.ElementViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ClassDiagram.UndoRedo.AddandRemove;
using ClassDiagram.View.UserControls;
using System;
using ClassDiagram.CopyPaste;
using ClassModel.Model;
using ClassModel.Model.Implementation;
using ClassModel.Model.Interfaces;

namespace ClassDiagram.ViewModel
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ICommand CanvasClickedCommand => new RelayCommand<CustomClickArgs>(CanvasClicked);
        public ICommand DeleteCommand => new RelayCommand(DeleteSelected);
        public ICommand CanvasOnMouseLeftBtnDownCommand
            => new RelayCommand<MouseButtonEventArgs>(CanvasOnMouseLeftBtnDown);
        public ICommand CanvasOnMouseMoveCommand => new RelayCommand<UIElement>(CanvasOnMouseMove);
        public ICommand CanvasOnMouseLeftBtnUpCommand => new RelayCommand<MouseButtonEventArgs>(CanvasOnMouseLeftUp);
        public ICommand WindowOnMouseLeftBtnDown => new RelayCommand<UIElement>(MoveFocusTo);
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand SaveDiagram => new RelayCommand(SaveDiagramClicked);    
        public ICommand LoadDiagram => new RelayCommand(LoadDiagramClicked);
        public ICommand SelectAllCommand => new RelayCommand(SelectAll);
        public ICommand PasteCommand => new RelayCommand(Paste);
        public ICommand SetMousePosCommand => new RelayCommand<UIElement>(SetMousePos);
        public ICommand CutCopyCommand => new RelayCommand<CopyPasteArgs>(CutCopy);

        private void CutCopy(CopyPasteArgs args)
        {
            var selectedBox = Boxes.First(box => box.BoxId == args.BoxId);

            if(args.Type == CopyPasteArgs.COPY)
                Copy(selectedBox);
            else if(args.Type == CopyPasteArgs.CUT)
                Cut(selectedBox);

            RaisePropertyChanged(nameof(CanPaste));
        }
        private void Cut(BoxViewModel selectedBox)
        {
            CopyPasteController.Instance.Cut(selectedBox, Boxes, Lines);
        }
         
        private void Copy(BoxViewModel selectedBox)
        {
            CopyPasteController.Instance.Copy(selectedBox, Boxes, Lines);
        }
        public ObservableCollection<BoxViewModel> Boxes { get; }
        public ObservableCollection<LineViewModel> Lines { get; }
        public CompositeCollection Elements { get; } = new CompositeCollection();

        public Diagram Diagram = new Diagram();
        private Point _mousePos;
        private Size _canvasSize;
        private UndoRedo.URController UndoRedo {get;}
        private BoxViewModel _fromBox;
        private int _boxCounter = 1;
        private Point? _initialMousePosition = null;
        private Point _startingOffset;
        private Point _startingPosition;
        private bool _hasMoved;
        private bool _isMoving;
        private BoxViewModel _clickedBox;

        #region propertiesForTheButtons

        private bool _isAddingClass;
        private bool _isAddingInterface;
        private bool _isAddingAbstractClass;
        private bool _isAddingAssosiation;
        private bool _isAddingDependency;
        private bool _isAddingAggregation;
        private bool _isAddingComposition;
        private bool _isAddingInheritance;
        private bool _isAddingRealization;

        public bool IsAddingClass
        {
            get { return _isAddingClass; }
            set { Set(ref _isAddingClass, !_isAddingClass && value); }
        }

        public bool IsAddingInterface
        {
            get { return _isAddingInterface; }
            set { Set(ref _isAddingInterface, !_isAddingInterface && value); }
        }

        public bool IsAddingAbstractClass
        {
            get { return _isAddingAbstractClass; }
            set { Set(ref _isAddingAbstractClass, !_isAddingAbstractClass && value); }
        }

        public bool IsAddingAssosiation
        {
            get { return _isAddingAssosiation; }
            set { Set(ref _isAddingAssosiation, !_isAddingAssosiation && value); }
        }

        public bool IsAddingDependency
        {
            get { return _isAddingDependency; }
            set { Set(ref _isAddingDependency, !_isAddingDependency && value); }
        }

        public bool IsAddingAggregation
        {
            get { return _isAddingAggregation; }
            set { Set(ref _isAddingAggregation, !_isAddingAggregation && value); }
        }

        public bool IsAddingComposition
        {
            get { return _isAddingComposition; }
            set { Set(ref _isAddingComposition, !_isAddingComposition && value); }
        }

        public bool IsAddingInheritance
        {
            get { return _isAddingInheritance; }
            set { Set(ref _isAddingInheritance, !_isAddingInheritance && value); }
        }

        public bool IsAddingRealization
        {
            get { return _isAddingRealization; }
            set { Set(ref _isAddingRealization, !_isAddingRealization && value); }
        }

        #endregion

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Boxes = new ObservableCollection<BoxViewModel>();
            Lines = new ObservableCollection<LineViewModel>();
            UndoRedo = ClassDiagram.UndoRedo.URController.Instance;
            UndoCommand = UndoRedo.UndoC;
            RedoCommand = UndoRedo.RedoC;


            Diagram.Boxes = new System.Collections.Generic.List<Box>();            
            Diagram.Lines = new System.Collections.Generic.List<Line>();

            Elements.Add(new CollectionContainer {Collection = Boxes});
            Elements.Add(new CollectionContainer {Collection = Lines});

            StatusBarViewModel.Instance.StatusBarMessage = "The program has been started";
            StatusBarViewModel.Instance.StatusBarCoordinates = new Point(175, 20);
        }

        public bool CanPaste
        {
            get { return CopyPasteController.Instance.CanPaste; }
        }

        private void SetMousePos(UIElement canvas)
        {
            _mousePos = Mouse.GetPosition(canvas);
            _canvasSize = canvas.RenderSize;
        }

        private void Paste()
        {
            CopyPaste.CopyPasteController.Instance.Paste(Boxes, Lines, _mousePos, _canvasSize);
        }

        private string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            // Set filter options and filter index.
            openFileDialog.Filter = "Xml Files (.xml)|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            openFileDialog.ShowDialog();

            string filename = openFileDialog.FileName.ToString();
            return filename;
        }

        private string SaveFileDialog()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            
            // Call the ShowDialog method to show the dialog box.
            saveFileDialog.ShowDialog();

            string filename = saveFileDialog.FileName.ToString() + ".xml";
            return filename;
        }


        private void LoadDiagramClicked()
        {
            string filename = OpenFileDialog();
            if (filename != "")
            {
                Diagram = Serializer.Serializer.Instance.DeserializeFromFile(filename);
                
                Boxes.Clear();
                foreach (Box _box in Diagram.Boxes)
                {
                    BoxViewModel newBox = new BoxViewModel(_box);
                    Boxes.Add(newBox);
                }

                Lines.Clear();
                foreach (Line _line in Diagram.Lines)
                {                // Find the box that the line starts from

                    LineViewModel newLine = new LineViewModel(_line, Boxes.Where(box => box.BoxId == _line.FromBoxId).First(), Boxes.Where(box => box.BoxId == _line.ToBoxId).First());
                    //LineViewModel newLine = new LineViewModel(_line);
                    Lines.Add(newLine);
                }
                StatusBarViewModel.Instance.StatusBarMessage = "The file has been loaded";
            }
        }

        private void SaveDiagramClicked()
        {
            string filename = SaveFileDialog();
            // Only store if filename is valid
            if (filename != "")
            {
                // Store boxes and lines in diagram class
                Diagram.Boxes.Clear();
                Diagram.Lines.Clear();

                foreach (BoxViewModel box in Boxes)
                {
                    Box _Box = new Box
                    {
                        BoxId = box.BoxId,                    
                        X = box.Position.X,
                        Y = box.Position.Y,
                        Label = box.Label,
                        Type = box.Type,
                        Height = box.Height,
                        FieldsList = box.FieldsList,
                        MethodList = box.MethodList,
                    };
                    Diagram.Boxes.Add(_Box); 
                }
                
                foreach (LineViewModel line in Lines)
                {
                    Line _Line = new Line
                    {
                        FromBoxId = line.FromBoxId,
                        ToBoxId = line.ToBoxId,
                        Type = line.Type,
                    };
                    Diagram.Lines.Add(_Line);
                }
                Serializer.Serializer.Instance.AsyncSerializeToFile(Diagram, filename);
                StatusBarViewModel.Instance.StatusBarMessage = $"The file has been saved as {filename}";
            }
            
        }

        private void CanvasOnMouseLeftBtnDown(MouseButtonEventArgs e)
        {
            var visual = e.Source as UIElement;
            if (visual == null) return;
            
            var point = Mouse.GetPosition(visual);
            
            _clickedBox = null;

            foreach (var box in Boxes)
            {
                if (box.IsPointInBox(point))
                    _clickedBox = box;
                if (box.IsSelected)
                {
                    box.StartingOffset = point;
                    box.StartingPosition = box.Position;
                }
            }

            if (_clickedBox != null)
            {
                _initialMousePosition = point;
                _startingOffset = new Point(point.X - _clickedBox.Position.X, point.Y - _clickedBox.Position.Y);
                _startingPosition = _clickedBox.Position;
                _isMoving = true;
            }
            else
            {
                DeselectAll();
            }

        }
        /// <summary>
        /// This method is responsible for checking the boxes are currently moving on the canvas
        /// If the boxes are moving, this method is also responsible for moving all of the boxes.
        /// </summary>
        /// <param name="visual">The UI elemnet which the mouse is moving in, this shuold always be the canvas</param>
        private void CanvasOnMouseMove(UIElement visual)
        {
            if (visual == null) return;

            var pos = Mouse.GetPosition(visual);
            StatusBarViewModel.Instance.StatusBarCoordinates = pos;

            if (!_isMoving || _clickedBox == null || _initialMousePosition == null) return;

            
            var gridSize = visual.RenderSize;

            if (_clickedBox.IsSelected)
            {
                var boxesToUpdate = new List<BoxViewModel>();
                var updateBoxes = true;
                foreach (var box in Boxes)
                {
                    if (!box.IsSelected || !box.StartingOffset.HasValue) continue;

                    var newPosition = new Point(pos.X - box.StartingOffset.Value.X,
                        pos.Y - box.StartingOffset.Value.Y);
                    if (newPosition.X < 0 || newPosition.X > gridSize.Width - box.Width || newPosition.Y < 0 ||
                        newPosition.Y > gridSize.Height - box.Height)
                    {
                        updateBoxes = false;
                        break;
                    }
                    else
                    {
                        boxesToUpdate.Add(box);
                    }
                }

                if (updateBoxes)
                {
                    foreach (var box in boxesToUpdate)
                    {
                        box.Position = new Point(pos.X - box.StartingOffset.Value.X, pos.Y - box.StartingOffset.Value.Y);
                    }
                }
                StatusBarViewModel.Instance.StatusBarMessage = "Boxes are moving";
            }
            else
            {
                var currentPoint = new Point(pos.X - _startingOffset.X, pos.Y - _startingOffset.Y);

                if (currentPoint.X > gridSize.Width - _clickedBox.Width)
                {
                    currentPoint.X = gridSize.Width - _clickedBox.Width;
                }
                if (currentPoint.Y > gridSize.Height - _clickedBox.Height)
                {
                    currentPoint.Y = gridSize.Height - _clickedBox.Height;
                }
                if (currentPoint.X < 0)
                {
                    currentPoint.X = 0;
                }
                if (currentPoint.Y < 0)
                {
                    currentPoint.Y = 0;
                }

                _clickedBox.Position = currentPoint;
                StatusBarViewModel.Instance.StatusBarMessage = "The box is moving";
            }

            _hasMoved = true;
        }

        

        /// <summary>
        /// This method is used to reset the starting offset on boxes if the boxes has been moved.
        /// Also it is used to make sure that a box which has just been moved is not nessesarily selected afterwords
        /// </summary>
        /// <param name="e">MouseButtonEventArgs specifying the even which happend</param>
        private void CanvasOnMouseLeftUp(MouseButtonEventArgs e)
        {
            if (_clickedBox != null && !_clickedBox.IsSelected && (_startingPosition.X != _clickedBox.Position.X || _startingPosition.Y != _clickedBox.Position.Y))
            {
                
                UndoRedo.Add(new MoveBox(_clickedBox, _startingPosition, _clickedBox.Position));
            }

            var movedBoxes = new List<BoxViewModel>();
            foreach (var box in Boxes)
            {
                if (box.StartingOffset.HasValue)
                {
                    if(_hasMoved)
                        movedBoxes.Add(box);
                    box.StartingOffset = null;
                    box.StartingPosition = null;
                }
            }

            if (_hasMoved)
            {
                if (_clickedBox != null && _clickedBox.IsSelected && ((_startingPosition.X - _clickedBox.Position.X) != 0 || (_startingPosition.Y - _clickedBox.Position.Y) != 0))
                {
                    UndoRedo.Add(new MoveMultipleBoxes(movedBoxes, _startingPosition.X - _clickedBox.Position.X, _startingPosition.Y - _clickedBox.Position.Y ));
                    StatusBarViewModel.Instance.StatusBarMessage = "Classes has been moved";
                    _startingPosition = new Point(0,0);
                }

                e.Handled = true;
            }
            _isMoving = false;
            _hasMoved = false;
        }
        /// <summary>
        /// This method selects all of the selected lines and boxes on the diagram
        /// </summary>
        private void SelectAll()
        {
            foreach (var line in Lines)
            {
                line.IsSelected = true;
            }

            foreach (var box in Boxes)
            {
                box.IsSelected = true;
            }
            StatusBarViewModel.Instance.StatusBarMessage = "Everything has been selected";
        }
        /// <summary>
        /// This method deselects all of the selected lines and boxes on the diagram
        /// </summary>
        private void DeselectAll()
        {
            foreach (var line in Lines)
            {
                line.IsSelected = false;
            }

            foreach (var box in Boxes)
            {
                box.IsSelected = false;
            }
            StatusBarViewModel.Instance.StatusBarMessage = "Removed selection from everything";
        }

        private void CanvasClicked(CustomClickArgs e)
        {
            var point = e.ClickedPoint;
            
            if (IsAddingClass || IsAddingAbstractClass || IsAddingInterface)
            {
                var newBox = new Box
                {
                    X = point.X,
                    Y = point.Y,
                    BoxId = Guid.NewGuid()                   
                };
                newBox.FieldsList.Add(new Fields("Insert Fields1"));
                newBox.MethodList.Add(new Methods("Insert Method1"));
                newBox.Label= "ClassName";

                if (IsAddingClass)
                {
                    newBox.Type = EBox.Class;
                    IsAddingClass = false;
                    StatusBarViewModel.Instance.StatusBarMessage = "A new class has been added";
                }
                else if (IsAddingAbstractClass)
                {
                    newBox.Type = EBox.Abstract;
                    IsAddingAbstractClass = false;
                    StatusBarViewModel.Instance.StatusBarMessage = "A new abstract class has been added";
                }
                else if (IsAddingInterface)
                {
                    newBox.Type = EBox.Interface;
                    IsAddingInterface = false;
                    StatusBarViewModel.Instance.StatusBarMessage = "A new interface has been added";
                }
                e.EventArgs.Handled = true;
                UndoRedo.AddExecute(new AddBox(Boxes, new BoxViewModel(newBox)));
            }
            else if (IsAddingAssosiation || IsAddingDependency || IsAddingAggregation || IsAddingComposition ||
                     IsAddingInheritance || IsAddingRealization)
            {
                if (_fromBox == null)
                    foreach (var boxViewModel in Boxes)
                    {
                        if (boxViewModel.IsPointInBox(point))
                        {
                            Debug.Print("Set from box");
                            _fromBox = boxViewModel;
                            _fromBox.IsSelected = false;
                            break;
                        }
                    }
                else
                    foreach (var boxViewModel in Boxes)
                    {
                        if (boxViewModel.IsPointInBox(point))
                        {
                            var lineViewModel = new LineViewModel(new Line() {FromBoxId = _fromBox.BoxId, ToBoxId = boxViewModel.BoxId}, _fromBox, boxViewModel);
                            if (IsAddingAssosiation)
                            {
                                lineViewModel.Type = ELine.Association;
                                IsAddingAssosiation = false;
                            }
                            else if (IsAddingDependency)
                            {
                                lineViewModel.Type = ELine.Dependency;
                                IsAddingDependency = false;
                            }
                            else if (IsAddingAggregation)
                            {
                                lineViewModel.Type = ELine.Aggregation;
                                IsAddingAggregation = false;
                            }
                            else if (IsAddingComposition)
                            {
                                lineViewModel.Type = ELine.Composition;
                                IsAddingComposition = false;
                            }
                            else if (IsAddingInheritance)
                            {
                                lineViewModel.Type = ELine.Inheritance;
                                IsAddingInheritance = false;
                            }
                            else if (IsAddingRealization)
                            {
                                lineViewModel.Type = ELine.Realization;
                                IsAddingRealization = false;
                            }

                            if (Lines.Any(line => (line.FromBoxId == _fromBox.BoxId && line.ToBoxId == boxViewModel.BoxId) || (line.FromBoxId == boxViewModel.BoxId && line.ToBoxId == _fromBox.BoxId)))
                            {
                                StatusBarViewModel.Instance.StatusBarMessage =
                                    "Cant add additional connections between two classes which has allready been connected";
                                return;
                            }
                            
                            UndoRedo.AddExecute(new AddLine(Lines, lineViewModel));
                            _fromBox = null;
                            StatusBarViewModel.Instance.StatusBarMessage = "Connection has been added";
                            break;
                        }
                    }
                e.EventArgs.Handled = true;
            }
        }

        private void DeleteSelected()
        {
            foreach (var line in Lines.Reverse())
            {
                if (line.IsSelected)
                    UndoRedo.AddExecute(new RemoveLine(Lines, line));

            }

            foreach (var box in Boxes.Reverse())
            {
                if (box.IsSelected)
                    UndoRedo.AddExecute(new RemoveBox(Boxes, Lines, box));

            }
        }
        
        /// <summary>
        /// This method changes focus to the specified UIElement.
        /// </summary>
        /// <param name="visualElement">This parameter represents the UIElement which will recieve focus, for this to work the UIElement has to be focusable</param>
        private void MoveFocusTo(UIElement visualElement)
        {
            visualElement.Focus();
        }
    }
}