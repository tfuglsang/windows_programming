using ClassDiagram.Model;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.UndoRedo.AddandRemove
{
    public class TextChanged : IURCommand
    {
        private readonly BoxViewModel _box;

        private Box newbox = new Box();
        private Box oldbox = new Box();

        public TextChanged(BoxViewModel box, Model.IBox old_settings, Model.IBox new_settings)
        {
            // Reference to BoxViewModel
            _box = box;

            CopyToLocalSettings(new_settings, newbox);
            CopyToLocalSettings(old_settings, oldbox);
        }

        public void Execute()
        {
            CopyLocalSettingsToBox(newbox);
        }

        public void UnExecute()
        {
            CopyLocalSettingsToBox(oldbox);
        }

        private void CopyToLocalSettings(IBox settings, Box box)
        {
            string s;
            foreach (Methods m in settings.MethodList)
            {
                s = m.Method;
                Methods method = new Methods(s);
                box.MethodList.Add(method);
            }

            foreach (Fields f in settings.FieldsList)
            {
                s = f.Field;
                Fields field = new Fields(s);
                box.FieldsList.Add(field);
            }

            // Copy the content of the label (de-reference)
            s = settings.Label;
            box.Label = s;
        }        

        private void CopyLocalSettingsToBox(Box settings)
        {
            string s;
            int i = 0;
            foreach (Fields f in settings.FieldsList)
            {
                s = f.Field;
                Fields field = new Fields(s);

                if (_box.FieldsList.Count - 1 < i)
                {
                    _box.FieldsList.Add(new Fields(""));
                    _box.FieldsList[i] = field;
                    _box.Height += 20;
                }
                else
                {
                    _box.FieldsList[i] = field;
                }
                i++;
            }

            // Remove excess fields from _box
            if (_box.FieldsList.Count > settings.FieldsList.Count)
            {
                int NumberOfFieldsToRemove = _box.FieldsList.Count - settings.FieldsList.Count;

                // Starting from the end of the list, remove element
                for (int c = 0; c < NumberOfFieldsToRemove; c++)
                {
                    _box.FieldsList.RemoveAt(_box.FieldsList.Count - c - 1);
                    _box.Height -= 20;
                }
            }

            i = 0;
            foreach (Methods m in settings.MethodList)
            {
                s = m.Method;
                Methods method = new Methods(s);

                if (_box.MethodList.Count - 1 < i)
                {
                    _box.MethodList.Add(new Methods(""));
                    _box.MethodList[i] = method;
                    _box.Height += 20;
                }
                else
                {
                    _box.MethodList[i] = method;
                }
                i++;
            }

            // Remove excess methods from _box
            if (_box.MethodList.Count > settings.MethodList.Count)
            {
                int NumberOfMethodsToRemove = _box.MethodList.Count - settings.MethodList.Count;

                // Starting from the end of the list, remove element
                for (int c = 0; c < NumberOfMethodsToRemove; c++)
                {
                    _box.MethodList.RemoveAt(_box.MethodList.Count - c - 1);
                    _box.Height -= 20;
                }
            }

            s = settings.Label;
            _box.Label = s;
        }

        



    }
}
