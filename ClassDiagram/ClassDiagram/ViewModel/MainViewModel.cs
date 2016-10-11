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

        public List<BoxViewModel> Boxes { get; }
        public List<LineViewModel> Lines { get; }
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
            Boxes = new List<BoxViewModel>();
            Lines = new List<LineViewModel>();
            Boxes.Add(new BoxViewModel(new Box()));

            Elements.Add(new CollectionContainer() { Collection = Boxes });
            Elements.Add(new CollectionContainer() { Collection = Lines });


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