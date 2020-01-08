using System;
using System.Text;
using System.Xml;
using DocumentFormat.OpenXml;
using System.IO;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace L
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingDir = Environment.CurrentDirectory;
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;

            printList(ExtractFromWord(projectDir + "/data/rv.docx"));
        }
        private static List<string> ExtractFromWord(string path)
        {
            List<string> list = new List<string>();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(path, false))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                foreach(OpenXmlElement element in body.ChildElements)
                {
                    list.Add(element.InnerText);
                }
            }
            return list;
        }

        private static void printList<T>(List<T> list)
        {
            foreach(T item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
