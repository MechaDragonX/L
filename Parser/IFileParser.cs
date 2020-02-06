using System.IO;

namespace L
{
    interface IFileParser
    {
        string[] ExtractAllLines(string path);
        string[] ExtractAllLines(Stream stream);
    }
}
