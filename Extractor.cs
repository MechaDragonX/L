using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
        /// Extract all text from a Microsoft Word document as a single string
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns></returns>
        public static string ExtractFromWord(string path)
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
        /// Extract each line of text in a Microsoft Word document to a string[]
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns></returns>
        public static string[] ExtractLinesFromWord(string path)
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
        /// Extract all text from a PDF document as a single string
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns></returns>
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
        /// Extract all text from a PDF document where each page is an item in a string[]
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns></returns>
        public static string[] ExtractPagesFromPDF(string path)
        {
            List<string> list = new List<string>();
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument pdf = new PdfLoadedDocument(stream);
            foreach (PdfPageBase page in pdf.Pages)
            {
                list.Add(page.ExtractText());
            }
            pdf.Close(true);
            stream.Close();
            return list.ToArray();
        }
        /// <summary>
        /// Extract each line of text in a PDF document to a string[]
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns></returns>
        public static string[] ExtractLinesFromPDF(string path)
        {
            string allText = ExtractFromPDF(path);
            return allText.Split('\n');
        }
    }
}
