using AODL.Document.Content;
using AODL.Document.TextDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L
{
    public class ParseOdt : IFileParser
    {
        /// <summary>
        /// Extract all text in a OpenDocument text file (*.odt)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>A single string with all text</returns>
        public string ExtractAllText(string path)
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
        public string[] ExtractAllLines(string path)
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
