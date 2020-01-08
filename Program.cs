using System;
using System.IO;
using System.Collections.Generic;

namespace L
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            Console.WriteLine(Extractor.ExtractFromWord(Path.Join(projectDir, "data", "rv.docx")));
        }
        private static void printList<T>(List<T> list)
        {
            foreach(T item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
