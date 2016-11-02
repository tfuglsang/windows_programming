using GalaSoft.MvvmLight;

namespace ClassDiagram.ViewModel.ElementViewModels
{
    public class ElementViewModel : ViewModelBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(ref _isSelected, value);
                RaisePropertyChanged(nameof(SelectedColor));
                RaisePropertyChanged(nameof(SelectedBorder));
            }
        }

        public string SelectedColor => _isSelected ? "Red" : "Black";
        public string SelectedBorder => _isSelected ? "Red" : "Transparent";
    }
}
