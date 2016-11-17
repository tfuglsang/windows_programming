using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClassDiagram.Model;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.CopyPaste
{
    class CopyPasteController
    {
        public static CopyPasteController Instance { get; } = new CopyPasteController();
        private Box copiedBox;
        private string copiedBoxAsString;
        private CopyPasteController()
        {
            
        }

        public void Copy(BoxViewModel box)
        {
            copiedBox = new Box
            {
                Number = box.Number,
                X = box.Position.X,
                Y = box.Position.Y,
                Label = box.Label,
                Type = box.Type,
                Height = box.Height,
                FieldsList = box.FieldsList,
                MethodList = box.MethodList,
            };

            copiedBox.FieldsStringList = new List<string>();
            foreach (Fields field in copiedBox.FieldsList)
            {
                copiedBox.FieldsStringList.Add(field.Field);
            }

            copiedBox.MethodStringList = new List<string>();
            foreach (Methods method in box.MethodList)
            {
                copiedBox.MethodStringList.Add(method.Method);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(copiedBox.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, copiedBox);
                copiedBoxAsString = textWriter.ToString();
            }
        }
   

        public BoxViewModel Paste()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(copiedBox.GetType());
            Box createdBox;

            using (StringReader stringReader = new StringReader(copiedBoxAsString))
            {
                createdBox = xmlSerializer.Deserialize(stringReader) as Box;
            }

            createdBox.FieldsList = new System.Collections.ObjectModel.ObservableCollection<Fields>();
            createdBox.MethodList = new System.Collections.ObjectModel.ObservableCollection<Methods>();
            foreach (string field in createdBox.FieldsStringList)
            {
                createdBox.FieldsList.Add(new Fields(field));
            }
            foreach (string method in createdBox.MethodStringList)
            {
                createdBox.MethodList.Add(new Methods(method));
            }

            return new BoxViewModel(createdBox);
        }



    }
}
