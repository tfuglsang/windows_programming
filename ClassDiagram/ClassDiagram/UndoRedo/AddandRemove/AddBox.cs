using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel;
using ClassDiagram.ViewModel.ElementViewModels;
using System.Collections.ObjectModel;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class AddBox : IURCommand
    {
        private ObservableCollection<BoxViewModel> boxes;
        private BoxViewModel box;

        public AddBox(ObservableCollection<BoxViewModel> _boxes, BoxViewModel _box)
        {
            boxes = _boxes;
            box = _box;
        }

        public void Execute()
        {
            boxes.Add(box);
        }

        public void UnExecute()
        {
            boxes.Remove(box);
        }
    }
}
