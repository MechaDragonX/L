using System;
using System.IO;
using System.Collections.Generic;

namespace L
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the resume parsing algorithm, L!");
			Console.WriteLine("This algorithm was written by Raghav Vivek and was named after an anime character who's known for deduction and detective skills.\n");
			Console.WriteLine("Please type the path to the file to parse:");
			string path = Console.ReadLine();

			// string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
			string[] lines = FileParser.ExtractAllLines(path);

			ResumeParser resumeParser = new ResumeParser(lines);
			Applicant applicant = resumeParser.Parse();
			applicant.Serialize(true);
			Console.WriteLine("\nAll done!! ^_^\nThe Applicant's data was written to the file \"<surname>, <given name>.json\" in a \"out\" folder in the provided folder!");
			Console.WriteLine("Thanks for using the program!");
			Console.ReadLine();
		}
	}
}
