using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class AddLine : IURCommand
    {
        private ObservableCollection<LineViewModel> lines;
        private LineViewModel line;

        public AddLine(ObservableCollection<LineViewModel> _lines, LineViewModel _line)
        {
            lines = _lines;
            line = _line;
        }

        public void Execute()
        {
            lines.Add(line);
        }

        public void UnExecute()
        {
            lines.Remove(line);
        }
    }
}
