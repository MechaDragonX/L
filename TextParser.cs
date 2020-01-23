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
                    // Make sure it's a header
                    if(Lines[i].Split().Length <= 3 && (current.Contains("skills") && current.Split(' ')[0] != "technical"))
                        startIndex++;
                }
                else
                {
                    // Make sure it's a header
                    if(Lines[i].Split().Length <= 3 && (current.Contains("skills") || current.Contains("experience")))
                        break;
                    else
                        skills.Add(Lines[i]);
                }
            }

            App.UnsortedSkills = skills.ToArray();
            return skills.ToArray();
        }
        /// <summary>
        /// Gets all technical skills from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>string[] where each skill is different element</returns>
        public string[] GetTechnicalSkills()
        {
            string current;
            int startIndex = -1;
            List<string> skills = new List<string>();
            for(int i = 0; i < Lines.Length; i++)
            {
                current = delimitersNoSpace.Replace(Lines[i].ToLower().Trim(), "");
                if (startIndex == -1)
                {
                    // Make sure it's a header
                    if (Lines[i].Split().Length <= 3 && (current.Contains("skills") && current.Contains("technical")))
                        startIndex++;
                }
                else
                {
                    // Make sure it's a header
                    if (Lines[i].Split().Length <= 3 && (current.Contains("skills") || current.Contains("experience")))
                        break;
                    else
                        skills.Add(Lines[i]);
                }
            }

            App.UnsortedSkills = skills.ToArray();
            return skills.ToArray();
        }
        public Applicant.Experience[] GetWorkExperience()
        {
            string current;
            int startIndex = -1;
            List<string> input = new List<string>();
            List<Applicant.Experience> experience = new List<Applicant.Experience>();
            for(int i = 0; i < Lines.Length; i++)
            {
                current = delimitersNoSpace.Replace(Lines[i].ToLower().Trim(), "");
                if(startIndex == -1)
                {
                    if(
                        Regex.IsMatch(current, @"\b(experience)\b") &&
                        Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b")
                        )
                    {
                        startIndex = i + 1;
                    }
                }
                else
                {
                    if (
                        Regex.IsMatch(current, @"\b(experience)\b") &&
                        Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b")
                        )
                    {
                        startIndex = -1;
                        experience.Add(GetExperience(input.ToArray()));
                    }
                    else
                    {
                        input.Add(Lines[i]);
                    }
                }
            }
            return experience.ToArray();
        }
        public Applicant.Experience GetExperience(string[] input)
        {
            Regex beforeAt = new Regex(@".*(?=(\sat))", RegexOptions.Compiled);
            Regex afterAt = new Regex(@"(?<=(at\s)).*", RegexOptions.Compiled);
            Regex beforeFor = new Regex(@".*(?=(\sfor))", RegexOptions.Compiled);
            Regex afterFor = new Regex(@"(?<=(for\s)).*", RegexOptions.Compiled);
            Regex beforeDash = new Regex(@".*(?=(\s-))", RegexOptions.Compiled);
            Regex afterDash = new Regex(@"(?<=(-\s)).*", RegexOptions.Compiled);

            string title = "";
            string location = "";
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            foreach(string element in input)
            {
                if((beforeAt.IsMatch(element) || beforeFor.IsMatch(element)) && title == "")
                    title = getTitle(element);
                if((afterAt.IsMatch(element) || afterFor.IsMatch(element)) && location == "")
                    location = getLocation(element);
                if(beforeDash.IsMatch(element))
                    startDate = getStartDate(element);
                if(afterDash.IsMatch(element))
                    endDate = getEndDate(element);
            }

            Applicant.Experience experience = new Applicant.Experience(title, location)
            {
                StartDate = startDate,
                EndDate = endDate,
                Responsibilities = getResponsibilites(input),
                Paid = getPaid(input)
            };

            return experience;
        }
        private string getTitle(string input)
        {
            if(Regex.IsMatch(input, @".*(?=(\sat))"))
                return Regex.Match(input, @".*(?=(\sat))").Value;
            if(Regex.IsMatch(input, @".*(?=(\sfor))"))
                return Regex.Match(input, @".*(?=(\sfor))").Value;
            return "";
        }
        private string getLocation(string input)
        {
            if(Regex.IsMatch(input, @"(?<=(at\s)).*"))
                return Regex.Match(input, @"(?<=(at\s)).*").Value;
            if(Regex.IsMatch(input, @"(?<=(for\s)).*"))
                return Regex.Match(input, @"(?<=(for\s)).*").Value;
            return "";
        }
        private DateTime getStartDate(string input)
        {
            if(Regex.IsMatch(input, @".*(?=(\s-))"))
                return DateTime.Parse(Regex.Match(input, @".*(?=(\s-))").Value);
            if(Regex.IsMatch(input, @".*(?=(\sto))"))
                return DateTime.Parse(Regex.Match(input, @".*(?=(\sto))").Value);
            return DateTime.MinValue;
        }
        private DateTime getEndDate(string input)
        {
            string match;
            if(Regex.IsMatch(input, @"(?<=(-\s)).*"))
            {
                match = Regex.Match(input, @"(?<=(-\s)).*").Value;
                if(match.ToLower() == "present" || match.ToLower() == "now")
                    return DateTime.Today;
                else
                    return DateTime.Parse(match);
            }
            return DateTime.MinValue;
        }
        private string[] getResponsibilites(string[] input)
        {
            List<string> responsibilities = new List<string>();
            Regex beforeAt = new Regex(@".*(?=(\sat))", RegexOptions.Compiled);
            Regex afterAt = new Regex(@"(?<=(at\s)).*", RegexOptions.Compiled);
            Regex beforeFor = new Regex(@".*(?=(\sfor))", RegexOptions.Compiled);
            Regex afterFor = new Regex(@"(?<=(for\s)).*", RegexOptions.Compiled);
            Regex beforeDash = new Regex(@".*(?=(\s-))", RegexOptions.Compiled);
            Regex afterDash = new Regex(@"(?<=(-\s)).*", RegexOptions.Compiled);
            Regex containsPay = new Regex(@"\b(pay)\b", RegexOptions.Compiled);

            foreach (string element in input)
            {
                if(
                    !beforeAt.IsMatch(element) && !afterAt.IsMatch(element) &&
                    !beforeFor.IsMatch(element) && !afterFor.IsMatch(element) &&
                    !beforeDash.IsMatch(element) && !afterDash.IsMatch(element) &&
                    !containsPay.IsMatch(element)
                    )
                    responsibilities.Add(element);
            }
            return responsibilities.ToArray();
        }
        private bool getPaid(string[] input)
        {
            foreach(string element in input)
            {
                if(Regex.IsMatch(element, @"\b(pay)\b") && !Regex.IsMatch(element, @"\b(without)\b"))
                    return true;
            }
            return false;
        }
    }
}
