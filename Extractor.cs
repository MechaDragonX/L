using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace L
{
    public static class Extractor
    {
        /// <summary>
        /// Extract all text from an non-XML-based Microsoft Word document (*.doc)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public static string ExtractFromWord(string path)
        {
            Application application = new Application();
            // Because "Document" exists in Interop.Word and OpenXml, I have to be explicit
            Microsoft.Office.Interop.Word.Document doc = application.Documents.Open(path);

            StringBuilder builder = new StringBuilder();    
            // Because "Document" exists in Interop.Word, OpenXml, and AODL, I have to be explicit
            foreach (Microsoft.Office.Interop.Word.Paragraph paragraph in doc.Content.Paragraphs)
            {
                builder.Append(paragraph.Range.Text);
            }
            application.Quit();
            return builder.ToString();
        }
        /// <summary>
        /// Extract all text from an XML-based Microsoft Word document (*.docx)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public static string ExtractFromWordXML(string path)
        {
            StringBuilder builder = new StringBuilder();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(path, false))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                foreach (OpenXmlElement element in body.ChildElements)
                {
                    builder.AppendFormat("{0}\n", element.InnerText);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        /// Extract each line in an XML-based Microsoft Word document (*.docx)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        public static string[] ExtractLinesFromWordXML(string path)
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
            return list.ToArray();
        }
        /// <summary>
        /// Extract all textin a PDF document
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public static string ExtractFromPDF(string path)
        {
            StringBuilder builder = new StringBuilder();
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument pdf = new PdfLoadedDocument(stream);
            foreach(PdfPageBase page in pdf.Pages)
            {
                builder.AppendFormat("{0}\n", page.ExtractText());
            }
            pdf.Close(true);
            stream.Close();
            return builder.ToString();
        }
        /// <summary>
        /// Extract each line in a PDF document
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        public static string[] ExtractLinesFromPDF(string path)
        {
            string allText = ExtractFromPDF(path);
            return allText.Split('\n');
        }
    }
}
