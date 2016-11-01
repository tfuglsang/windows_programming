using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ClassDiagram.Model;
using ClassDiagram.ViewModel.ElementViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ClassDiagram.UndoRedo.AddandRemove;

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
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        //public List<BoxViewModel> BoxesList { get; }
        //public List<LineViewModel> LinesList { get; }
        //public CollectionContainer Boxes { get; }
        //public CollectionContainer Lines { get; }

        public ObservableCollection<BoxViewModel> Boxes { get; }
        public ObservableCollection<LineViewModel> Lines { get; }
        public CompositeCollection Elements { get; } = new CompositeCollection();
        private UndoRedo.URController UndoRedo {get;}
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
            UndoRedo = ClassDiagram.UndoRedo.URController.Instance;
            UndoCommand = UndoRedo.UndoC;
            RedoCommand = UndoRedo.RedoC;

            //var boxbox = new Box {X = 400, Y = 400, Number = _boxCounter++};
            //var boxboxViewModel = new BoxViewModel(boxbox);
            //var secondBox = new Box {X = 200, Y = 200, Number = _boxCounter++};
            //var secondBoxViewModel = new BoxViewModel(secondBox);

            //Boxes.Add(boxboxViewModel);
            //Boxes.Add(secondBoxViewModel);

            Elements.Add(new CollectionContainer {Collection = Boxes});
            Elements.Add(new CollectionContainer {Collection = Lines});
        }

        private void CanvasClicked(Point point)
        {
            Debug.Print($"{point.X},{point.Y}"); // debug information

            foreach (var line in Lines)
            {
                line.IsSelected = false;
            }

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

                //Boxes.Add(new BoxViewModel(newBox));
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
                            //Lines.Add(lineViewModel);
                            UndoRedo.AddExecute(new AddLine(Lines, lineViewModel));
                            _fromBox = null;
                            break;
                        }
                    }
            }
        }
    }
}