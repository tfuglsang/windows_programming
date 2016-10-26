using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class MoveBox : IURCommand
    {
        private readonly BoxViewModel _box;
        private Point _newPos;
        private Point _oldPos;


        public MoveBox(BoxViewModel box, Point oldPos, Point newPos)
        {
            _box = box;
            _oldPos = oldPos;
            _newPos = newPos;
        }

        public void Execute()
        {
            Point coordinates = new Point(0, 0);
            coordinates.X = _newPos.X;
            coordinates.Y = _newPos.Y;
            _box.Position = coordinates;
        }

        public void UnExecute()
        {
            Point coordinates = new Point(0, 0);
            coordinates.X = _oldPos.X;
            coordinates.Y = _oldPos.Y;
            _box.Position = coordinates;
        }
    }
}
