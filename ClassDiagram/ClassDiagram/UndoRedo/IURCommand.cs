using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.UndoRedo
{
    public interface IURCommand
    {
        void Execute();

        void UnExecute();
    }
}
