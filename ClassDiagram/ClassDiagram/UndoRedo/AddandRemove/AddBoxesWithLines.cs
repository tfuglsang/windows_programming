using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class AddBoxesWithLines : IURCommand
    {
        private ObservableCollection<BoxViewModel> _boxes;
        private ObservableCollection<LineViewModel> _lines;
        private List<BoxViewModel> _boxesToAdd;
        private List<LineViewModel> _linesToAdd;

        public AddBoxesWithLines(ObservableCollection<BoxViewModel> boxes, ObservableCollection<LineViewModel> lines, List<BoxViewModel> boxesToAdd, List<LineViewModel> linesToAdd)
        {
            _boxes = boxes;
            _lines = lines;
            _boxesToAdd = boxesToAdd;
            _linesToAdd = linesToAdd;
        }
        public void Execute()
        {
            if(_boxesToAdd == null)
                throw new Exception("Boxes are not allowed to be null");

            foreach (var boxViewModel in _boxesToAdd)
            {
                _boxes.Add(boxViewModel);
            }

            if (_linesToAdd == null) return;
            foreach (var lineViewModel in _linesToAdd)
            {
                _lines.Add(lineViewModel);
            }
        }

        public void UnExecute()
        {
            if (_boxesToAdd == null)
                throw new Exception("Boxes are not allowed to be null");

            if (_linesToAdd != null) 
                foreach (var lineViewModel in _linesToAdd)
                {
                    _lines.Remove(lineViewModel);
                }

            foreach (var boxViewModel in _boxesToAdd)
            {
                _boxes.Remove(boxViewModel);
            }

            
        }
    }
}
