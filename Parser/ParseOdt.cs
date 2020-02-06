using AODL.Document.Content;
using AODL.Document.TextDocuments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace L
{
    public class ParseOdt : IFileParser
    {
        /// <summary>
        /// Extract each line in a OpenDocument text document (*.odt)
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
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

        /// <summary>
        /// Extract each line in a OpenDocument text document (*.odt)
        /// </summary>
        /// <param name="stream">File's IO Stream</param>
        /// <returns>Each line as a string[] element</returns>
        public string[] ExtractAllLines(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
