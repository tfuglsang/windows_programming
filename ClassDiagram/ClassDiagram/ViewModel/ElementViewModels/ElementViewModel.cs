using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class ElementViewModel : ViewModelBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
              return _isSelected;  
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(propertyName: nameof(IsSelected));
            }
        }
    }
}
