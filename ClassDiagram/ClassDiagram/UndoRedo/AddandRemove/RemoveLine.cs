using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class RemoveLine : IURCommand
    {
        private ObservableCollection<LineViewModel> lines;
        private List<LineViewModel> removeLines;

        public RemoveLine(ObservableCollection<LineViewModel> _lines, List<LineViewModel> _removeLines)
        {
            lines = _lines;
            removeLines = _removeLines;
        }

        public RemoveLine(ObservableCollection<LineViewModel> _lines, LineViewModel _removeLine)
        {
            lines = _lines;
            removeLines = new List<LineViewModel>();
            removeLines.Add(_removeLine);
        }

        public void Execute()
        {
            removeLines.ForEach(x => lines.Remove(x));
        }

        public void UnExecute()
        {
            removeLines.ForEach(x => lines.Add(x));
        }
    }
}
