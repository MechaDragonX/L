using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace L
{
    public class ParsePdf : IFileParser
    {
        /// <summary>
        /// Extract all text in a PDF document
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public string ExtractAllText(string path)
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
        public string[] ExtractAllLines(string path)
        {
            string allText = ExtractAllText(path);
            return allText.Split('\n');
        }
    }
}
