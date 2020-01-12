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

			// Console.WriteLine(Extractor.ExtractAll("filename"));
			// printArray(Extractor.ExtractAllLines("filename"));

			// Applicant name = Applicant.Deserialize("filename"));
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
