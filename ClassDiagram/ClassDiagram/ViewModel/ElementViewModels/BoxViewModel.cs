using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using ClassDiagram.Model;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class BoxViewModel : ElementViewModel
    {
        private IBox _box;

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
            get { return new Point(_box.X, _box.Y); 
            }
            set
            {
                _box.X = value.X;
                _box.Y = value.Y;
                RaisePropertyChanged();
            }
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

        public List<String> FieldsList
        {
            get { return _box.FieldsList; }
            set
            {
                _box.FieldsList = value;
                RaisePropertyChanged();
            }
        }

        public List<String> MethodList
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

            _box.FieldsList = new List<string>();
            _box.MethodList = new List<string>();

            // For test purposes
            _box.FieldsList.Add("int Alpha");
            _box.FieldsList.Add("float Bravo");
            _box.FieldsList.Add("double Caesar");
            _box.MethodList.Add("int getAlpha()");
            _box.MethodList.Add("float getBravo()");
            _box.MethodList.Add("double getCaesar()");

        }
    }
}
