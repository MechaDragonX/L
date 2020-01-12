using System;
using System.Collections.Generic;
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

        private static Regex emailPattern = new Regex(
            "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])",
            RegexOptions.Compiled
        );
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
        /// Gets the name of the applicant from the text of their resume and adds to the applicant object
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
        public string getEmail()
        {
            string emailLine = "";
            string email = "";
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
    }
}
