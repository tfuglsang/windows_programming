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
using ClassDiagram.ViewModel;
using ClassDiagram.ViewModel.ElementViewModels;

namespace ClassDiagram.CopyPaste
{
    public class CopyPasteController
    {
        public static CopyPasteController Instance { get; } = new CopyPasteController();
        private string _copiedDiagramString;
        private Point _positionOfSelectedBox;
        public bool CanPaste => !string.IsNullOrEmpty(_copiedDiagramString);

        private CopyPasteController()
        {
            
        }

        private Diagram GenerateDiagram(BoxViewModel selectedBox, ObservableCollection<BoxViewModel> boxes, ObservableCollection<LineViewModel> lines)
        {
            var diagramToCopy = new Diagram();

            if (selectedBox.IsSelected)
            {
                diagramToCopy.Boxes = new List<Box>();
                var tempGuidList = new List<Guid>();
                foreach (var boxViewModel in boxes)
                {
                    if (boxViewModel.IsSelected)
                    {
                        tempGuidList.Add(boxViewModel.BoxId);
                        diagramToCopy.Boxes.Add(boxViewModel.Box as Box);
                    }
                }

                diagramToCopy.Lines = new List<Line>();
                foreach (var lineViewModel in lines)
                {
                    if (tempGuidList.Contains(lineViewModel.FromBoxId) && tempGuidList.Contains(lineViewModel.ToBoxId))
                        diagramToCopy.Lines.Add((Line)lineViewModel.Line);
                }
            }
            else
            {
                diagramToCopy.Boxes = new List<Box>();
                diagramToCopy.Boxes.Add((Box)selectedBox.Box);
            }

            return diagramToCopy;
        }

        private string SerializeDiagramToString(Diagram diagramToSerialize)
        {
            foreach (var box in diagramToSerialize.Boxes)
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
                xmlSerializer.Serialize(textWriter, diagramToSerialize);
                return textWriter.ToString();
            }
        }

        public void Cut(BoxViewModel selectedBox, ObservableCollection<BoxViewModel> boxes,
            ObservableCollection<LineViewModel> lines)
        {
            var diagram = GenerateDiagram(selectedBox, boxes, lines);

            var boxesToDelete = diagram.Boxes.Select(box => boxes.First(boxViewModel => boxViewModel.BoxId == box.BoxId)).ToList();

            _positionOfSelectedBox = new Point(selectedBox.Position.X, selectedBox.Position.Y);
            _copiedDiagramString = SerializeDiagramToString(diagram);

            UndoRedo.URController.Instance.AddExecute(new RemoveBox(boxes, lines, boxesToDelete));
        }

        public void Copy(BoxViewModel selectedBox, ObservableCollection<BoxViewModel> boxes, ObservableCollection<LineViewModel> lines)
        {
            var diagram = GenerateDiagram(selectedBox, boxes, lines);
            _positionOfSelectedBox = new Point(selectedBox.Position.X, selectedBox.Position.Y);
            
            _copiedDiagramString = SerializeDiagramToString(diagram);
            
        }
   

        public void Paste(ObservableCollection<BoxViewModel> boxes, ObservableCollection<LineViewModel> lines, Point mousePosition, Size canvasSize)
        {
            if(string.IsNullOrEmpty(_copiedDiagramString))
                throw new Exception("Can't paste when the diagram is null");

            var xmlSerializer = new XmlSerializer(typeof(Diagram));

            Diagram diagramToPaste;

            using (var stringReader = new StringReader(_copiedDiagramString))
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

                var newPosition = new Point(box.X - _positionOfSelectedBox.X + mousePosition.X, box.Y - _positionOfSelectedBox.Y + mousePosition.Y);

                if (newPosition.X < 0 || newPosition.X > canvasSize.Width - box.Width || newPosition.Y < 0 ||
                    newPosition.Y > canvasSize.Height - box.Height)
                {
                    StatusBarViewModel.Instance.StatusBarMessage = "Cant add paste boxes outside of the canvas";
                    return;
                }

                var boxViewModel = new BoxViewModel(box) {Position = newPosition};

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
