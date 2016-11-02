using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClassDiagram.UndoRedo
{

    public class URController
    {
        private static readonly URController _here = new URController();
        private readonly Stack<IURCommand> _uStack = new Stack<IURCommand>();
        private readonly Stack<IURCommand> _rStack = new Stack<IURCommand>();

        private URController() : base()
        {
            UndoC = new RelayCommand(Undo, CanUndo);
            RedoC = new RelayCommand(Redo, CanRedo);
        }

        public static URController Instance => _here;

        public RelayCommand UndoC { get; } 
        public RelayCommand RedoC { get; }

        private void UpdateCommandStatus()
        {
            UndoC.RaiseCanExecuteChanged();
            RedoC.RaiseCanExecuteChanged();
        }

        public bool CanUndo() => _uStack.Any();
        public void Undo()
        {
            IURCommand instruct = _uStack.Pop();
            _rStack.Push(instruct);
            instruct.UnExecute();
            UpdateCommandStatus();
        }

        public bool CanRedo() => _rStack.Any();
        public void Redo()
        {
            IURCommand instruct = _rStack.Pop();
            _uStack.Push(instruct);
            instruct.Execute();
            UpdateCommandStatus();
        }

        public void AddExecute(IURCommand instruct)
        {
            _uStack.Push(instruct);
            _rStack.Clear();
            instruct.Execute();
            UpdateCommandStatus();
        }

    }
}
