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
