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
            { ".txt", new ParseTxt() },
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
               throw new FileNotFoundException("The file was not found!");

            if(!parsers.ContainsKey(Path.GetExtension(path)))
               throw new FileFormatException($"File type \"{Path.GetExtension(path)}\" is not supported!");

            IFileParser parser = parsers[Path.GetExtension(path)];
            return parser.ExtractAllLines(path);
        }

        /// <summary>
        /// Extract each line from a given file
        /// </summary>
        /// <param name="stream">/The file's IO Stream</param>
        /// <param name="key">The file's key (the name)</param>
        /// <returns>Each line as a string[] element</returns>
        public static string[] ExtractAllLinesFromS3(Stream stream, string key)
        {
            if(!parsers.ContainsKey(Path.GetExtension(key)))
                throw new FileFormatException($"File type \"{Path.GetExtension(key)}\" is not supported!");

            IFileParser parser = parsers[Path.GetExtension(key)];
            return parser.ExtractAllLines(stream);
        }
    }
}
