using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassDiagram.ViewModel.ElementViewModels;
using ClassModel.Model;
using ClassModel.Model.Implementation;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    class ChangeBoxType : IURCommand
    {
        private BoxViewModel box;
        private EBox oldType;
        private EBox newType;

        public ChangeBoxType(BoxViewModel box, EBox newType)
        {
            this.box = box;
            this.oldType = this.box.Type;
            this.newType = newType;
        }
        public void Execute()
        {
            box.Type = newType;
        }

        public void UnExecute()
        {
            box.Type = oldType;
        }
    }
}
