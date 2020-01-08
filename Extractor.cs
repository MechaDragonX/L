using AODL.Document.Content;
using AODL.Document.TextDocuments;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace L
{
    public static class Extractor
    {
        /// <summary>
        /// Extract all text from a given file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public static string ExtractAll(string path)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException();

            return(Path.GetExtension(path)) switch
            {
                ".txt" => File.ReadAllText(path),
                ".doc" => ExtractFromWord(path),
                ".docx" => ExtractFromWordXML(path),
                ".pdf" => ExtractFromPDF(path),
                ".odt" => ExtractFromODT(path),
                _ => throw new FileFormatException(),
            };
        }
        /// <summary>
        /// Extract each line from a given file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        public static string[] ExtractAllLines(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            return (Path.GetExtension(path)) switch
            {
                ".txt" => File.ReadAllLines(path),
                // ".doc" => ExtractLinesFromWord(path),
                ".docx" => ExtractLinesFromWordXML(path),
                ".pdf" => ExtractLinesFromPDF(path),
                ".odt" => ExtractLinesFromODT(path),
                _ => throw new FileFormatException(),
            };
        }

        /// <summary>
        /// Extract all text from an non-XML-based Microsoft Word document (*.doc)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        private static string ExtractFromWord(string path)
        {
            Application application = new Application();
            // Because "Document" exists in Interop.Word and OpenXml, I have to be explicit
            Microsoft.Office.Interop.Word.Document doc = application.Documents.Open(path);

            StringBuilder builder = new StringBuilder();
            // Because "Paragraph" exists in Interop.Word, OpenXml, and AODL, I have to be explicit
            foreach (Microsoft.Office.Interop.Word.Paragraph paragraph in doc.Content.Paragraphs)
            {
                builder.Append(paragraph.Range.Text);
            }

            application.Quit();
            doc.Close();
            return builder.ToString();
        }
        /// <summary>
        /// Extract all text from an XML-based Microsoft Word document (*.docx)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        private static string ExtractFromWordXML(string path)
        {
            StringBuilder builder = new StringBuilder();
            using(WordprocessingDocument doc = WordprocessingDocument.Open(path, false))
            {
                string text = string.Join("\n", doc.MainDocumentPart.Document.Body.ChildElements.Select(x => x.InnerText));
                builder.Append(text);
            }
            return builder.ToString();
        }
        /// <summary>
        /// Extract each line in an XML-based Microsoft Word document (*.docx)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        private static string[] ExtractLinesFromWordXML(string path)
        {
            IEnumerable<string> list;
            using(WordprocessingDocument doc = WordprocessingDocument.Open(path, false))
            {
                list = doc.MainDocumentPart.Document.Body.ChildElements.Select(x => x.InnerText);
            }
            return list.ToArray();
        }
        /// <summary>
        /// Extract all text in a PDF document
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        private static string ExtractFromPDF(string path)
        {
            StringBuilder builder = new StringBuilder();
            using(FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                PdfLoadedDocument pdf = new PdfLoadedDocument(stream);
                foreach(PdfPageBase page in pdf.Pages)
                {
                    builder.AppendFormat("{0}\n", page.ExtractText());
                }
                pdf.Close(true);
            }
            return builder.ToString();
        }
        /// <summary>
        /// Extract each line in a PDF document
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        private static string[] ExtractLinesFromPDF(string path)
        {
            string allText = ExtractFromPDF(path);
            return allText.Split('\n');
        }
        /// <summary>
        /// Extract all text in a OpenDocument text file (*.odt)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        private static string ExtractFromODT(string path)
        {
            StringBuilder builder = new StringBuilder();
            using(TextDocument doc = new TextDocument())
            {
                doc.Load(path);
                string text = string.Join("\n", doc.Content.Cast<IContent>().Select(x => x.Node.InnerText));
                builder.Append(text);
            }
            return builder.ToString();
        }
        private static string[] ExtractLinesFromODT(string path)
        {
            IEnumerable<string> list = new List<string>();
            using(TextDocument doc = new TextDocument())
            {
                doc.Load(path);
                IEnumerable<string> text = doc.Content.Cast<IContent>().Select(x => x.Node.InnerText);
            }
            return list.ToArray();
        }
    }
}
