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

			TextParser textParser = new TextParser(FileParser.ExtractAllLines("filename"));
			string name = textParser.GetName();
			Console.WriteLine(name);
			string email = textParser.GetEmail();
			Console.WriteLine(email);
			string phoneNumber = textParser.GetPhoneNumber();
			Console.WriteLine(phoneNumber);
			string address = textParser.GetAddress();
			Console.WriteLine(address);
			string summary = textParser.GetSummary();
			Console.WriteLine(summary);
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
