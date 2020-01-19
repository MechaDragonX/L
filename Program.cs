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

			// Console.WriteLine(FileParser.ExtractAll("filename"));
			// printArray(FileParser.ExtractAllLines("filename"));

			// Applicant name = Applicant.Deserialize("filename"));

			TextParser textParser = new TextParser("filename");
			string name = textParser.getName();
			Console.WriteLine(name);
			string email = textParser.getEmail();
			Console.WriteLine(email);
			string phoneNumber = textParser.getPhoneNumber();
			Console.WriteLine(phoneNumber);
			string address = textParser.getAddress();
			Console.WriteLine(address);
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
