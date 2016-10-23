using System;
using System.Collections.Generic;
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
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ICommand CanvasClickedCommand => new RelayCommand<Point>(CanvasClicked);

        //public List<BoxViewModel> BoxesList { get; }
        //public List<LineViewModel> LinesList { get; }
        //public CollectionContainer Boxes { get; }
        //public CollectionContainer Lines { get; }

        public ObservableCollection<BoxViewModel> Boxes{ get; }
        public ObservableCollection<LineViewModel> Lines{ get; }
        public CompositeCollection Elements { get; } = new CompositeCollection();

        #region propertiesForTheButtons
        private bool _isAddingClass;
        private bool _isAddingInterface;
        private bool _isAddingAbstractClass;
        private bool _isAddingAssosiation;
        private bool _isAddingDirAssosiation;
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
        public bool IsAddingDirAssosiation
        {
            get { return _isAddingDirAssosiation; }
            set { Set(ref _isAddingDirAssosiation, !_isAddingDirAssosiation && value); }
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
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Boxes = new ObservableCollection<BoxViewModel>();
            Lines = new ObservableCollection<LineViewModel>();

            Box boxbox = new Box() { Width = 800, X = 400, Y = 400 };
            Boxes.Add(new BoxViewModel(boxbox));

            boxbox = new Box() { Width = 800, X = 200, Y = 200 };
            Boxes.Add(new BoxViewModel(boxbox));


            Elements.Add(new CollectionContainer() { Collection = Boxes });
            Elements.Add(new CollectionContainer() { Collection = Lines });

        }

        private void CanvasClicked(Point point)
        {
            Debug.Print($"{point.X},{point.Y}");
            if (_isAddingClass)
            {
                Box newBox = new Box() {Width = 800, X = point.X, Y=point.Y };
                Boxes.Add(new BoxViewModel(newBox));
                
                IsAddingClass = false;
            }
        }
        
    }
}