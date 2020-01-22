using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace L
{
    public class TextParser
    {
        private static readonly Regex delimitersNoSpace = new Regex("[—;:/\\<>~*]+", RegexOptions.Compiled);
        private static readonly Regex delimitersWithSpace = new Regex("[\\s—;:/\\<>~*]+", RegexOptions.Compiled);
        // private static char[] delimiters = new char[] { '—', ';', ':', '/', '\\', '<', '>', '~', '*' };
        private static readonly Regex emailPattern = new Regex(
            "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])",
            RegexOptions.Compiled
        );
        private static readonly Regex phonePattern = new Regex("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}", RegexOptions.Compiled);

        public Applicant App { get; }
        public string[] Lines { get; }

        private TextParser()
        {
            App = new Applicant();
        }
        public TextParser(string[] lines) : this()
        {
            Lines = lines;
        }

        /// <summary>
        /// Gets the name of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Name in "[Given Name] [Surname]" format</returns>
        public string GetName()
        {
            string[] givenAndSurname = Lines[0].Split(' ');
            App.GivenName = givenAndSurname[0];
            App.Surname = givenAndSurname[1];

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}, {1}", App.Surname, App.GivenName);
            return builder.ToString();
        }
        /// <summary>
        /// Gets the email of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>email</returns>
        public string GetEmail()
        {
            string emailLine = "";
            string email;
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
            App.Email = email;
            return email;
        }
        /// <summary>
        /// Gets the phone number of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Phone Number</returns>
        public string GetPhoneNumber()
        {
            string numberLine = "";
            string phoneNumber = "";
            foreach(string line in Lines)
            {
                if(line.Where(x => char.IsDigit(x)).Count() >= 10)
                {
                    numberLine = line;
                    break;
                }
            }
            if(!phonePattern.IsMatch(numberLine))
                return "";
            phoneNumber = phonePattern.Match(numberLine).Value;
            App.PhoneNumber = phoneNumber;
            return phoneNumber;
        }
        /// <summary>
        /// Gets the address of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Address</returns>
        public string GetAddress()
        {
            // string addressLine = "";
            string[] elements = null;
            string address = "";
            foreach(string line in Lines)
            {
                // if(Regex.Match(line, @"/[,]+/g").Groups.Count == 2)
                if(line.Split(',').Length == 3)
                {
                    elements = delimitersNoSpace.Split(line);
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
            App.Address = address;
            return address;
        }
        /// <summary>
        /// Gets the summary from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Summary</returns>
        public string GetSummary()
        {
            List<string> current;
            string[] summArray = null;
            string summary = "";
            for(int i = 0; i < Lines.Length; i++)
            {
                current = Lines[i].Split(' ').ToList();
                if (current[0].ToLower().Contains("summary") || current[0].ToLower().Contains("objective"))
                {
                    if (current.Count > 1)
                    {
                        current.RemoveAt(0);
                        current.CopyTo(summArray);
                        break;
                    }
                    else
                    {
                        summary = Lines[i + 1];
                    }
                }
            }
            if(summArray == null)
            {
                App.Summary = summary;
                return summary;
            }

            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < summArray.Length; i++)
            {
                if(i == summArray.Length - 1)
                    builder.Append(summArray[i]);
                else
                    builder.AppendFormat("{0} ", summArray[i]);
            }
            summary = builder.ToString();
            App.Summary = summary;
            return summary;
        }
        /// <summary>
        /// Gets the high school from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>High School</returns>
        public string GetHighSchool()
        {
            foreach(string line in Lines)
            {
                if(line.ToLower().Contains("high school") && line.Split(' ').Length <= 8) // length of 8 to allow things like "Bob A. Ferguson High School, Grand Rapids, MI" (This school most likely doesn't exist)
                {
                    App.HighSchool = line;
                    return line;
                }
            }
            return "";
        }
        /// <summary>
        /// Gets the undergraduate college from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Undergraduate College</returns>
        public string GetCollegeUG()
        {
            string currentLineLower;
            foreach(string line in Lines)
            {
                currentLineLower = line.ToLower();
                if (currentLineLower.Contains("bachelor of") ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(bs)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(b.s.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(ba)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(b.a.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(bfa)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(b.f.a.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    currentLineLower.Contains("undergraduate") || currentLineLower.Contains("undergrad") ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(ug)(?:\b|\W)", RegexOptions.IgnoreCase)
                    )
                {
                    App.CollegeUG = line;
                    return line;
                }
            }
            return "";
        }
        /// <summary>
        /// Gets the postgraduate college from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Postgraduate College</returns>
        public string GetCollegePG()
        {
            string currentLineLower;
            foreach(string line in Lines)
            {
                currentLineLower = line.ToLower();
                if(currentLineLower.Contains("master of") ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(ms)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(m.s.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(ma)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(m.a.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(mfa)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(m.f.a.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    currentLineLower.Contains("postgraduate") || currentLineLower.Contains("postgrad") ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(pg)(?:\b|\W)", RegexOptions.IgnoreCase)
                    )
                {
                    App.CollegeUG = line;
                    return line;
                }
            }
            return "";
        }
        /// <summary>
        /// Gets all unsorted skills from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>string[] where each skill is different element</returns>
        public string[] GetUnsortedSkills()
        {
            string current;
            int startIndex = -1;
            List<string> skills = new List<string>();
            for(int i = 0; i < Lines.Length; i++)
            {
                current = delimitersNoSpace.Replace(Lines[i].ToLower().Trim(), "");
                if(startIndex == -1)
                {
                    if(current.Contains("skills") && current.Split(' ')[0] != "technical")
                    {
                        startIndex = i;
                    }
                }
                else
                {
                    if(current.Contains("skills") || current.Contains("experience"))
                    {
                        break;
                    }
                    else
                        skills.Add(Lines[i]);
                }
            }

            App.UnsortedSkills = skills.ToArray();
            return skills.ToArray();
        }
    }
}
