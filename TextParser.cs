using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace L
{
    public class TextParser
    {
        public enum TextType
        {
            Single,
            Lines
        }

        private static Regex delimiters = new Regex("[—;:/\\<>~*]+");
        // private static char[] delimiters = new char[] { '—', ';', ':', '/', '\\', '<', '>', '~', '*' };
        private static Regex emailPattern = new Regex(
            "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])",
            RegexOptions.Compiled
        );
        private static Regex phonePattern = new Regex("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}", RegexOptions.Compiled);

        public Applicant applicant { get; }
        public TextType Type { get; }
        public string Text { get; }
        public string[] Lines { get; }

        private TextParser(TextType type)
        {
            Type = type;
            applicant = new Applicant();
        }
        public TextParser(TextType type, string text) : this(type)
        {
            Text = text;
        }
        public TextParser(TextType type, string[] lines) : this(type)
        {
            Lines = lines;
        }

        /// <summary>
        /// Gets the name of the applicant from the text of their resume and adds it to the applicant object
        /// </summary>
        /// <returns>Name in "[Given Name] [Surname]" format</returns>
        public string getName()
        {
            string name;
            string[] givenAndSurname;
            if(Type == TextType.Lines)
            {
                name = Regex.Replace(Lines[0], @"/\n\r/", "");
                givenAndSurname = name.Split(' ');
                applicant.GivenName = givenAndSurname[0];
                applicant.Surname = givenAndSurname[1];
                return name;
            }

            return "";
        }
        /// <summary>
        /// Gets the email of the applicant from the text of their resume and adds it to the applicant object
        /// </summary>
        /// <returns>email</returns>
        public string getEmail()
        {
            string emailLine = "";
            string email;
            if(Type == TextType.Lines)
            {
                foreach(string line in Lines)
                {
                    if(line.Contains('@'))
                    {
                        emailLine = line;
                        break;
                    }
                }
                if(!emailPattern.IsMatch(emailLine))
                    return "";
                email = emailPattern.Match(emailLine).Value;
                applicant.Email = email;
                return email;
            }
            return "";
        }
        /// <summary>
        /// Gets the phone number of the applicant from the text of their resume and adds it to the applicant object
        /// </summary>
        /// <returns>Phone Number</returns>
        public string getPhoneNumber()
        {
            string numberLine = "";
            string phoneNumber = "";
            if(Type == TextType.Lines)
            {
                foreach(string line in Lines)
                {
                    if(line.Where(x => char.IsDigit(x)).Count() >= 10)
                    {
                        numberLine = line;
                        break;
                    }
                }
                if (!phonePattern.IsMatch(numberLine))
                    return "";
                phoneNumber = phonePattern.Match(numberLine).Value;
                applicant.PhoneNumber = phoneNumber;
                return phoneNumber;
            }
            return "";
        }
        /// <summary>
        /// Gets the address of the applicant from the text of their resume and adds it to the applicant object
        /// </summary>
        /// <returns>Address</returns>
        public string getAddress()
        {
            // string addressLine = "";
            string[] elements = null;
            string address = "";
            if(Type == TextType.Lines)
            {
                foreach(string line in Lines)
                {
                    // if(Regex.Match(line, @"/[,]+/g").Groups.Count == 2)
                    if(line.Split(',').Length == 3)
                    {
                        elements = delimiters.Split(line);
                        break;
                    }
                }
                foreach(string element in elements)
                {
                    // if(Regex.Match(element, @"/[,]+/g").Groups.Count == 2)
                    if(element.Split(',').Length == 3)
                    {
                        address = element.Trim();
                        break;
                    }
                }
                applicant.Address = address;
                return address;
            }
            return "";
        }
    }
}
