using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ClassDiagram.Model;
using ClassDiagram.ViewModel.ElementViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

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
        public ICommand CanvasClickedCommand => new RelayCommand<Point>(CanvasClicked);
        public ICommand SaveDiagram => new RelayCommand(SaveDiagramClicked);
        public ICommand LoadDiagram => new RelayCommand(LoadDiagramClicked);        

        public ObservableCollection<BoxViewModel> Boxes { get; }
        public ObservableCollection<LineViewModel> Lines { get; }
        public CompositeCollection Elements { get; } = new CompositeCollection();

        public Diagram Diagram = new Diagram();

        private BoxViewModel _fromBox;
        private int _boxCounter = 1;
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

        private void CanvasClicked(Point point)
        {
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

                Boxes.Add(new BoxViewModel(newBox));
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

                            foreach (var line in Lines)
                            {
                                if ((line.FromNumber == _fromBox.Number && line.ToNumber == boxViewModel.Number) || (line.FromNumber == boxViewModel.Number && line.ToNumber == _fromBox.Number))
                                    return; // TODO Let the user know that the action is not allowed instead of just ignoring the action!!
                            }
                            Lines.Add(lineViewModel);
                            _fromBox = null;
                            break;
                        }
                    }
            }
        }
    }
}