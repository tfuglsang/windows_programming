using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel.ElementViewModels;
using ClassModel.Model;
using ClassModel.Model.Interfaces;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    class ChangeLineType : IURCommand
    {
        private LineViewModel line;
        private ELine oldType;
        private ELine newType;

        public ChangeLineType(LineViewModel line, ELine newType)
        {
            this.line = line;
            this.oldType = line.Type;
            this.newType = newType;
        }
        public void Execute()
        {
            line.Type = newType;
        }

        public void UnExecute()
        {
            line.Type = oldType;
        }
    }
}
