using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Text;

namespace L
{
    public class ParseDoc : IFileParser
    {
        /// <summary>
        /// Extract all text from an non-XML-based Microsoft Word document (*.doc)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        //public string ExtractAllText(string path)
        //{
        //    Application application = new Application();
        //    Document doc = application.Documents.Open(path);

        //    StringBuilder builder = new StringBuilder();
        //    foreach(Paragraph paragraph in doc.Content.Paragraphs)
        //    {
        //        builder.Append(paragraph.Range.Text);
        //    }

        //    application.Quit();
        //    doc.Close();
        //    return builder.ToString();
        //}
        public string[] ExtractAllLines(string path)
        {
            throw new NotImplementedException();
        }
        public string[] ExtractAllLines(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
