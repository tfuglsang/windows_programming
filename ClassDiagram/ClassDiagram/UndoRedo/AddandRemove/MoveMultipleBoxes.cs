using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel.ElementViewModels;
using System.Windows;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class MoveMultipleBoxes : IURCommand
    {
        private List<BoxViewModel> boxes;

        private double xOffset;
        private double yOffset;

        public MoveMultipleBoxes(List<BoxViewModel> _boxes, double _xOffset, double _yOffset)
        {
            boxes = _boxes;
            xOffset = _xOffset;
            yOffset = _yOffset;
        }

        public void Execute()
        {
            Point coordinates = new Point(0, 0);
            for(int i = 0; i < boxes.Count; i++)
            {
                coordinates.X = xOffset;
                coordinates.Y = yOffset;
                boxes[i].Position = coordinates;
            }
        }

        public void UnExecute()
        {
            Point coordinates = new Point(0, 0);
            for (int i = 0; i < boxes.Count; i++)
            {
                coordinates.X = xOffset;
                coordinates.Y = yOffset;
                boxes[i].Position = coordinates;
            }
        }

    }
}
