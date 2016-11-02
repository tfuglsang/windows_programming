using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class RemoveBox : IURCommand
    {
        private ObservableCollection<BoxViewModel> boxes;
        private ObservableCollection<LineViewModel> lines;
        private List<BoxViewModel> removeBoxes;
        private List<LineViewModel> removeLines;

        public RemoveBox(ObservableCollection<BoxViewModel> _boxes, ObservableCollection<LineViewModel> _lines, List<BoxViewModel> _removeBoxes)
        {
            boxes = _boxes;
            lines = _lines;
            removeBoxes = _removeBoxes;
            removeLines = _lines.Where(x => _removeBoxes.Any(y => y.Number == x.From.Number || y.Number == x.To.Number)).ToList();
        }

        public RemoveBox(ObservableCollection<BoxViewModel> _boxes, ObservableCollection<LineViewModel> _lines,
            BoxViewModel _removeBox)
        {
            boxes = _boxes;
            lines = _lines;
            removeBoxes = new List<BoxViewModel>();
            removeBoxes.Add(_removeBox);
            removeLines = _lines.Where(x => removeBoxes.Any(y => y.Number == x.From.Number || y.Number == x.To.Number)).ToList();
        }

        public void Execute()
        {
            removeLines.ForEach(x => lines.Remove(x));
            removeBoxes.ForEach(x => boxes.Remove(x));
        }

        public void UnExecute()
        {
            removeBoxes.ForEach(x => boxes.Add(x));
            removeLines.ForEach(x => lines.Add(x));
        }
    }
}
