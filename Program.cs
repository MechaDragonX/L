﻿using System;
using System.IO;
using System.Collections.Generic;

namespace L
{
	class Program
	{
		static void Main(string[] args)
		{
			string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

			// PrintArray(FileParser.ExtractAllLines("filename"));

			// Applicant name = Applicant.Deserialize("filename"));

			TextParser textParser = new TextParser(FileParser.ExtractAllLines(Path.Join(projectDir, "data", "rv.docx")));
			//string name = textParser.GetName();
			//Console.WriteLine(name);
			//string email = textParser.GetEmail();
			//Console.WriteLine(email);
			//string phoneNumber = textParser.GetPhoneNumber();
			//Console.WriteLine(phoneNumber);
			//string address = textParser.GetAddress();
			//Console.WriteLine(address);
			//string summary = textParser.GetSummary();
			//Console.WriteLine(summary);
			//string highSchool = textParser.GetHighSchool();
			//Console.WriteLine(highSchool);
			//string collegeUG = textParser.GetCollegeUG();
			//Console.WriteLine(collegeUG);
			//string collegePG = textParser.GetCollegePG();
			//Console.WriteLine(collegePG);
			//string[] unsortedSkills = textParser.GetUnsortedSkills();
			//PrintArray(unsortedSkills);
			//string[] technicalSkills = textParser.GetTechnicalSkills();
			//PrintArray(technicalSkills);

			string[] input = new string[]
			{
				"Tutor at Mathnasium",
				"July 2019 - Present",
				"Helped students with special math coursework",
				"Worked for pay twice a week"
			};
			Applicant.Experience experience = textParser.GetExperience(input);
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
