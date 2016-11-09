using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using ClassDiagram.Model;

namespace ClassDiagram.Serializer
{
    public class Serializer
    {
        public static Serializer Instance { get; } = new Serializer();

        private Serializer() { }

        public async void AsyncSerializeToFile(Diagram diagram, string path)
        {
            await Task.Run(() => SerializeToFile(diagram, path));
        }

        private void SerializeToFile(Diagram diagram, string path)
        {
            using (FileStream stream = File.Create(path))
            {
                // Extract strings from observable collection and add them to string list so they can be serialized
                int ff = 0;
                foreach(Box box in diagram.Boxes)
                {
                    diagram.Boxes[ff].FieldsStringList = new List<string>();                    
                    foreach (Fields field in box.FieldsList)
                    {
                        diagram.Boxes[ff].FieldsStringList.Add(field.Field);                        
                    }

                    diagram.Boxes[ff].MethodStringList = new List<string>();
                    foreach (Methods method in box.MethodList)
                    {
                        diagram.Boxes[ff].MethodStringList.Add(method.Method);
                    }

                    ff++;
                }                
                
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                serializer.Serialize(stream, diagram);
            }
        }

        public Task<Diagram> AsyncDeserializeFromFile(string path)
        {
            return Task.Run(() => DeserializeFromFile(path));
        }

        public Diagram DeserializeFromFile(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                Diagram diagram = serializer.Deserialize(stream) as Diagram;

                int ff = 0;
                foreach (Box box in diagram.Boxes)
                {
                    diagram.Boxes[ff].FieldsList = new System.Collections.ObjectModel.ObservableCollection<Fields>();
                    diagram.Boxes[ff].MethodList = new System.Collections.ObjectModel.ObservableCollection<Methods>();
                    foreach (string field in box.FieldsStringList)
                    {                        
                        diagram.Boxes[ff].FieldsList.Add(new Fields(field));                        
                    }
                    foreach (string method in box.MethodStringList)
                    {
                        diagram.Boxes[ff].MethodList.Add(new Methods(method));
                    }
                    ff++;
                }
                return diagram;
            }
        }
    }
}
