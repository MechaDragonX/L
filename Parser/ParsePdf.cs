using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace L
{
    public class ParsePdf : IFileParser
    {
        /// <summary>
        /// Extract each line in a PDF document
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        public string[] ExtractAllLines(string path)
        {
            StringBuilder builder = new StringBuilder();
            using(FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                PdfLoadedDocument pdf = new PdfLoadedDocument(stream);
                foreach(PdfPageBase page in pdf.Pages)
                {
                    builder.Append(page.ExtractText());
                }
                pdf.Close(true);
            }
            return Regex.Split(builder.ToString(), "\n\r");
        }

        /// <summary>
        /// Extract each line in a PDF document
        /// </summary>
        /// <param name="stream">File's IO Stream</param>
        /// <returns>Each line as a string[] element</returns>
        public string[] ExtractAllLines(Stream stream)
        {
            StringBuilder builder = new StringBuilder();
            using(stream)
            {
                PdfLoadedDocument pdf = new PdfLoadedDocument(stream);
                foreach(PdfPageBase page in pdf.Pages)
                {
                    builder.Append(page.ExtractText());
                }
                pdf.Close(true);
            }
            return Regex.Split(builder.ToString(), "\n\r");
        }
    }
}
