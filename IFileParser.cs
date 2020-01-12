namespace L
{
    interface IFileParser
    {
        public string ExtractAllText(string path);
        public string[] ExtractAllLines(string path);
    }
}
