using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace L
{
    public class ParseTxt : IFileParser
    {
        public string[] ExtractAllLines(string path)
        {
            return File.ReadAllLines(path);
        }
        public string[] ExtractAllLines(Stream stream)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            return lines.ToArray();
        }
    }
}
