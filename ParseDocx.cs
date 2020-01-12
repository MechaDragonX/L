using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L
{
    public class ParseDocx : IFileParser
    {
        /// <summary>
        /// Extract all text from an XML-based Microsoft Word document (*.docx)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public string ExtractAllText(string path)
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
        public string[] ExtractAllLines(string path)
        {
            IEnumerable<string> list;
            using(WordprocessingDocument doc = WordprocessingDocument.Open(path, false))
            {
                list = doc.MainDocumentPart.Document.Body.ChildElements.Select(x => x.InnerText);
            }
            return list.ToArray();
        }
    }
}
