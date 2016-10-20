using System;
using System.Collections.Generic;
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
        public ICommand MouseLeftClick => new RelayCommand<MouseButtonEventArgs>(handleMouseClick);

        //public List<BoxViewModel> BoxesList { get; }
        //public List<LineViewModel> LinesList { get; }
        //public CollectionContainer Boxes { get; }
        //public CollectionContainer Lines { get; }

        public List<BoxViewModel> Boxes{ get; }
        public List<LineViewModel> Lines{ get; }
        public CompositeCollection Elements { get; } = new CompositeCollection();

        private bool _isAddingBox;

        public bool IsAddingBox
        {
            get
            {
                return _isAddingBox;
            }
            set
            {
                _isAddingBox = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            //BoxesList = new List<BoxViewModel>();
            //LinesList = new List<LineViewModel>();

            //Box boxbox = new Box() { Width = 300, X = 400, Y = 400 };
            //BoxesList.Add(new BoxViewModel(boxbox));
            //Box _box = new Box();
            //_box.X = 400;
            //_box.Y = 200;
            //_box.Width = 300;
            //BoxesList.Add(new BoxViewModel(_box));

            //Boxes = new CollectionContainer() { Collection = BoxesList };
            //Lines = new CollectionContainer() { Collection = LinesList };





            Boxes = new List<BoxViewModel>();
            Lines = new List<LineViewModel>();

            Box boxbox = new Box() { Width = 800, X = 400, Y = 400 };
            Boxes.Add(new BoxViewModel(boxbox));

            boxbox = new Box() { Width = 800, X = 200, Y = 200 };
            Boxes.Add(new BoxViewModel(boxbox));


            Elements.Add(new CollectionContainer() { Collection = Boxes });
            Elements.Add(new CollectionContainer() { Collection = Lines });

            //Elements.Add(new CollectionContainer() {Collection = Boxes});
            //Elements.Add(new CollectionContainer() { Collection = Lines });
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private void handleMouseClick(MouseButtonEventArgs args)
        {
            

        }
    }
}