using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Amazon.S3;

namespace L
{
    public static class FileParser
    {
        private static Dictionary<string, IFileParser> parsers = new Dictionary<string, IFileParser>()
        {
            { ".txt", null },
            { ".doc", new ParseDoc() },
            { ".docx", new ParseDocx() },
            { ".pdf", new ParsePdf() },
            { ".odt", new ParseOdt() }
        };

        /// <summary>
        /// Extract each line from a given file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Each line as a string[] element</returns>
        public static string[] ExtractAllLines(string path)
        {
            if(!File.Exists(path))
               throw new FileNotFoundException();

            if(!parsers.ContainsKey(Path.GetExtension(path)))
               throw new FileFormatException();

            var parser = parsers[Path.GetExtension(path)];
            if(parser == null)
                return File.ReadAllLines(path);
            return parser.ExtractAllLines(path);
        }

        /// <summary>
        /// Extract each line from a given file
        /// </summary>
        /// <param name="key">The file's key (the name)</param>
        /// <returns>Each line as a string[] element</returns>
        public static string[] ExtractAllLinesFromS3(string key)
        {
            if(!File.Exists(key))
               throw new FileNotFoundException();

            if(!parsers.ContainsKey(Path.GetExtension(key)))
               throw new FileFormatException();

            var parser = parsers[Path.GetExtension(key)];
            if (parser == null)
                return File.ReadAllLines(key);
            return parser.ExtractAllLines(key);
        }
    }
}
