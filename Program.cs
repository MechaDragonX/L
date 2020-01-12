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

			// Console.WriteLine(FileParser.ExtractAll("filename"));
			// printArray(FileParser.ExtractAllLines("filename"));

			// Applicant name = Applicant.Deserialize("filename"));

			TextParser textParser = new TextParser(TextParser.TextType.Lines, FileParser.ExtractAllLines(Path.Join(projectDir, "data", "rv.docx")));
			string name = textParser.getName();
			Console.WriteLine(name);
			string email = textParser.getEmail();
			Console.WriteLine(email);
		}
		private static void printArray<T>(T[] array)
		{
			foreach (T item in array)
			{
				Console.WriteLine(item);
			}
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
