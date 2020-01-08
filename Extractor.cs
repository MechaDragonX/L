using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace L
{
    public static class Extractor
    {
        public static List<string> ExtractFromWord(string path)
        {
            List<string> list = new List<string>();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(path, false))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                foreach (OpenXmlElement element in body.ChildElements)
                {
                    list.Add(element.InnerText);
                }
            }
            return list;
        }
    }
}
