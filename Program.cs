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
			string[] lines = FileParser.ExtractAllLines("filename");
			// PrintArray(lines);

			// Applicant name = Applicant.Deserialize("filename"));

			ResumeParser resumeParser = new ResumeParser(lines);
			Applicant applicant = resumeParser.Parse();
			Console.WriteLine(applicant);
		}
		private static void PrintArray<T>(T[] array)
		{
			foreach (T item in array)
			{
				Console.WriteLine(item);
			}
		}
		private static void PrintList<T>(List<T> list)
		{
			foreach(T item in list)
			{
				Console.WriteLine(item);
			}
		}
	}
}
