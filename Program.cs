﻿using System;
using System.IO;
using System.Collections.Generic;

namespace L
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the resume parsing algorithm, L!");
			Console.WriteLine("This algorithm was written by Raghav Vivek and was named after an anime character who's known for deuction and detective skills.\n");
			Console.WriteLine("Please type the path to the file to parse:");
			string path = Console.ReadLine();

			// string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
			string[] lines = FileParser.ExtractAllLines(path);
			// PrintArray(lines);

			// Applicant name = Applicant.Deserialize("filename"));

			ResumeParser resumeParser = new ResumeParser(lines);
			Applicant applicant = resumeParser.Parse();
			// Console.WriteLine(applicant);
			applicant.Serialize();
			Console.WriteLine("\nAll done!! ^_^\nThe Applicant's data was written to the file \"<surname>, <given name>.json\"!");
			Console.WriteLine("Thanks for using the program!");
			Console.ReadLine();
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
