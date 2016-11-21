using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using ClassDiagram.Model;
using ClassDiagram.UndoRedo.AddandRemove;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.CopyPaste
{
    public class CopyPasteController
    {
        public static CopyPasteController Instance { get; } = new CopyPasteController();
        private Box copiedBox;
        private string copiedDiagramString;
        private CopyPasteController()
        {
            
        }

        public void Copy(Diagram diagram)
        {
            foreach (var box in diagram.Boxes)
            {
                box.FieldsStringList = new List<string>();
                foreach (var field in box.FieldsList)
                {
                    box.FieldsStringList.Add(field.Field);
                }

                box.MethodStringList = new List<string>();
                foreach (var method in box.MethodList)
                {
                    box.MethodStringList.Add(method.Method);
                }
            }
            
            var xmlSerializer = new XmlSerializer(typeof(Diagram));

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, diagram);
                copiedDiagramString = textWriter.ToString();
            }
        }
   

        public void Paste(ObservableCollection<BoxViewModel> boxes, ObservableCollection<LineViewModel> lines, Point mousePosition, Size canvasSize)
        {
            if(string.IsNullOrEmpty(copiedDiagramString))
                throw new Exception("Can't paste when the diagram is null");

            var xmlSerializer = new XmlSerializer(typeof(Diagram));

            Diagram diagramToPaste;

            using (var stringReader = new StringReader(copiedDiagramString))
            {
                diagramToPaste = xmlSerializer.Deserialize(stringReader) as Diagram;
            }

            // Dictionary to map between new and old GUIDs
            var oldGuidToNewGuid = new Dictionary<Guid, Guid>();

            var boxesToAdd = new List<BoxViewModel>();
            var linesToAdd = new List<LineViewModel>();

            foreach (var box in diagramToPaste.Boxes)
            {
                var newBoxId = Guid.NewGuid();
                oldGuidToNewGuid.Add(box.BoxId, newBoxId);

                box.BoxId = newBoxId;

                box.FieldsList = new ObservableCollection<Fields>();
                box.MethodList = new ObservableCollection<Methods>();
                foreach (var field in box.FieldsStringList)
                {
                    box.FieldsList.Add(new Fields(field));
                }
                foreach (var method in box.MethodStringList)
                {
                    box.MethodList.Add(new Methods(method));
                }

                var boxViewModel = new BoxViewModel(box) {Position = mousePosition};

                boxesToAdd.Add(boxViewModel); 
            }

            foreach (var line in diagramToPaste.Lines)
            {
                Guid newFromBoxId;
                Guid newToBoxId;
                if(!oldGuidToNewGuid.TryGetValue(line.FromBoxId, out newFromBoxId))
                    throw new Exception("Unknown from box GUID!!");
                if(!oldGuidToNewGuid.TryGetValue(line.ToBoxId, out newToBoxId))
                    throw new Exception("Unknown to box GUID!!!");

                line.FromBoxId = newFromBoxId;
                line.ToBoxId = newToBoxId;

                linesToAdd.Add(new LineViewModel(line, boxesToAdd.First(box => box.BoxId == newFromBoxId), boxesToAdd.First(box => box.BoxId == newToBoxId)));
            }
            
            UndoRedo.URController.Instance.AddExecute(new AddBoxesWithLines(boxes, lines, boxesToAdd, linesToAdd));
        }



    }
}
