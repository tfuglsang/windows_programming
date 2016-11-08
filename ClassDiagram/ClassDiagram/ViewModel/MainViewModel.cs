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
using ClassDiagram.Model;

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

        public ObservableCollection<BoxViewModel> Boxes { get; }
        public ObservableCollection<LineViewModel> Lines { get; }
        public CompositeCollection Elements { get; } = new CompositeCollection();

        public Diagram Diagram = new Diagram();
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
        }

        private void LoadDiagramClicked()
        {
            Diagram = Serializer.Serializer.Instance.DeserializeFromFile("C:\\temp/serial.xml");

            Boxes.Clear();
            foreach (Box _box in Diagram.Boxes)
            {
                BoxViewModel newBox = new BoxViewModel(_box);
                Boxes.Add(newBox);
            }

            Lines.Clear();
            foreach (Line _line in Diagram.Lines)
            {
                // Find the box that the line starts from



                LineViewModel newLine = new LineViewModel(_line, Boxes[_line.FromNumber-1] , Boxes[_line.ToNumber-1]);
                //LineViewModel newLine = new LineViewModel(_line);
                Lines.Add(newLine);
            }
        }

        private void SaveDiagramClicked()
        {
            // Store boxes and lines in diagram class
            Diagram.Boxes.Clear();
            Diagram.Lines.Clear();
            foreach (CollectionContainer collection in Elements)
            {
                try
                {
                    foreach (BoxViewModel boxes in collection.Collection)
                    {
                        Box _Box = new Box
                        {
                            Number = boxes.Number,
                            Height = boxes.Height,
                            Width = boxes.Width,
                            X = boxes.Position.X,
                            Y = boxes.Position.Y,
                            Label = boxes.Label,
                            FieldsList = boxes.FieldsList,
                            MethodList = boxes.MethodList,
                        };

                        Diagram.Boxes.Add(_Box);
                        var x = collection.Collection.GetType();
                    }
                }
                catch (System.Exception e)
                {
                    // Invalid cast..
                }
                try
                {
                    foreach (LineViewModel lines in collection.Collection)
                    {
                        Line _Line = new Line
                        {
                            FromNumber = lines.FromNumber,
                            ToNumber = lines.ToNumber,
                            Type = lines.Type,
                        };
                        Diagram.Lines.Add(_Line);

                    }
                }
                catch (System.Exception e)
                {                   
                    // Invalid cast..
                }
            }

            // Serialize and save to .xml file                
            System.IO.Directory.CreateDirectory("C:\\temp");
            Serializer.Serializer.Instance.AsyncSerializeToFile(Diagram, "C:\\temp/serial.xml");
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
            if (!_isMoving || _clickedBox == null || _initialMousePosition == null) return;

            var pos = Mouse.GetPosition(visual);
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
            if (_clickedBox != null && !_clickedBox.IsSelected)
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
                if (_clickedBox != null && _clickedBox.IsSelected)
                {
                    UndoRedo.Add(new MoveMultipleBoxes(movedBoxes, _startingPosition.X - _clickedBox.Position.X, _startingPosition.Y - _clickedBox.Position.Y ));
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
        }

        private void CanvasClicked(CustomClickArgs e)
        {
            var point = e.ClickedPoint;

            Debug.Print($"{point.X},{point.Y}"); // debug information
            
            if (IsAddingClass || IsAddingAbstractClass || IsAddingInterface)
            {
                var newBox = new Box
                {
                    X = point.X,
                    Y = point.Y,
                    Number = _boxCounter++
                };

                if (IsAddingClass)
                {
                    newBox.Type = EBox.Class;
                    IsAddingClass = false;
                }
                else if (IsAddingAbstractClass)
                {
                    newBox.Type = EBox.Abstract;
                    IsAddingAbstractClass = false;
                }
                else if (IsAddingInterface)
                {
                    newBox.Type = EBox.Interface;
                    IsAddingInterface = false;
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
                            var lineViewModel = new LineViewModel(new Line(), _fromBox, boxViewModel);
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
                                IsAddingInterface = false;
                            }
                            else if (IsAddingRealization)
                            {
                                lineViewModel.Type = ELine.Realization;
                                IsAddingRealization = false;
                            }

                            if (Lines.Any(line => (line.FromNumber == _fromBox.Number && line.ToNumber == boxViewModel.Number) || (line.FromNumber == boxViewModel.Number && line.ToNumber == _fromBox.Number)))
                            {
                                return; // TODO Let the user know that the action is not allowed instead of just ignoring the action!!
                            }
                            UndoRedo.AddExecute(new AddLine(Lines, lineViewModel));
                            _fromBox = null;
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