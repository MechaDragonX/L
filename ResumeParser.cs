using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace L
{
    public class ResumeParser
    {
        private static readonly Regex delimitersNoSpace = new Regex("[—;:/\\<>~*]+", RegexOptions.Compiled);
        private static readonly Regex delimitersWithSpace = new Regex("[\\s—;:/\\<>~*]+", RegexOptions.Compiled);
        // private static char[] delimiters = new char[] { '—', ';', ':', '/', '\\', '<', '>', '~', '*' };
        private static readonly Regex emailPattern = new Regex(
            "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])",
            RegexOptions.Compiled
        );
        private static readonly Regex phonePattern = new Regex("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}", RegexOptions.Compiled);

        public string[] Lines { get; }

        /// <summary>
        /// Create a new Resume Parser
        /// </summary>
        public ResumeParser() { }
        /// <summary>
        /// Create a new Resume Parser
        /// </summary>
        /// <param name="lines">All the lines from the resume to parse</param>
        public ResumeParser(string[] lines)
        {
            Lines = lines;
        }

        /// <summary>
        /// Creates a new applicant from teh data of the resume that was passed into Resume Parser
        /// </summary>
        /// <returns></returns>
        public Applicant Parse()
        {
            Applicant applicant = new Applicant();
            try
            {
                // Profile Info
                var name = GetName();
                applicant.GivenName = name.Item1;
                applicant.Surname = name.Item2;

                applicant.Email = GetEmail();
                applicant.PhoneNumber = GetPhoneNumber();
                applicant.Address = GetAddress();

                // Summary/Objective
                applicant.Summary = GetSummary();

                // Basic Education Details
                applicant.HighSchool = GetHighSchool();
                applicant.CollegeUG = GetCollegeUG();
                applicant.CollegePG = GetCollegePG();

                // Skills
                applicant.UnsortedSkills = GetUnsortedSkills();
                applicant.TechnicalSkills = GetTechnicalSkills();

                // Experience
                applicant.EducationalXP = GetEducationalExperience();
                applicant.WorkXP = GetWorkExperience();
                applicant.VolunteerXP = GetVolunteerExperience();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return applicant;
        }

        /// <summary>
        /// Gets the name of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Name in "[Given Name] [Surname]" format</returns>
        private Tuple<string, string> GetName()
        {
            string[] givenAndSurname = Lines[0].Split(' ');
            return new Tuple<string, string>(givenAndSurname[0], givenAndSurname[1]);
        }
        /// <summary>
        /// Gets the email of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>email</returns>
        private string GetEmail()
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
            return email;
        }
        /// <summary>
        /// Gets the phone number of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Phone Number</returns>
        private string GetPhoneNumber()
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
            return phoneNumber;
        }
        /// <summary>
        /// Gets the address of the applicant from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Address</returns>
        private string GetAddress()
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
            return address;
        }
        /// <summary>
        /// Gets the summary from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Summary</returns>
        private string GetSummary()
        {
            List<string> current;
            string[] summArray = null;
            string summary = "";
            for(int i = 0; i < Lines.Length; i++)
            {
                current = Lines[i].Split(' ').ToList();
                if(current[0].ToLower().Contains("summary") || current[0].ToLower().Contains("objective"))
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
            return builder.ToString();
        }
        /// <summary>
        /// Gets the high school from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>High School</returns>
        private string GetHighSchool()
        {
            foreach(string line in Lines)
            {
                if(line.ToLower().Contains("high school") && line.Split(' ').Length <= 8) // length of 8 to allow things like "Bob A. Ferguson High School, Grand Rapids, MI" (This school most likely doesn't exist)
                    return line;
            }
            return "";
        }
        /// <summary>
        /// Gets the undergraduate college from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Undergraduate College</returns>
        private string GetCollegeUG()
        {
            string currentLineLower;
            foreach(string line in Lines)
            {
                currentLineLower = line.ToLower();
                if(currentLineLower.Contains("bachelor of") ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(bs)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(b.s.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(ba)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(b.a.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(bfa)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(b.f.a.)(?:\b|\W)", RegexOptions.IgnoreCase) ||
                    currentLineLower.Contains("undergraduate") || currentLineLower.Contains("undergrad") ||
                    Regex.IsMatch(currentLineLower, @"(?:\b|\W)(ug)(?:\b|\W)", RegexOptions.IgnoreCase)
                    )
                    return line;
            }
            return "";
        }
        /// <summary>
        /// Gets the postgraduate college from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>Postgraduate College</returns>
        private string GetCollegePG()
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
                    return line;
            }
            return "";
        }
        /// <summary>
        /// Gets all unsorted skills from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>string[] where each skill is different element</returns>
        private string[] GetUnsortedSkills()
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
            return skills.ToArray();
        }
        /// <summary>
        /// Gets all technical skills from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>string[] where each skill is different element</returns>
        private string[] GetTechnicalSkills()
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
                    if(Lines[i].Split().Length <= 3 && (current.Contains("skills") && current.Contains("technical")))
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
            return skills.ToArray();
        }
        /// <summary>
        /// Gets all Work Experience from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>An array of each experience entry</returns>
        private Applicant.Experience[] GetWorkExperience()
        {
            string current;
            int startIndex = -1;
            bool started = false;
            List<string> input = new List<string>();
            List<Applicant.Experience> experience = new List<Applicant.Experience>();
            for(int i = 0; i < Lines.Length; i++)
            {
                current = delimitersNoSpace.Replace(Lines[i].ToLower().Trim(), "");
                if(startIndex == -1)
                {
                    if(experience.Count == 0)
                    {
                        if(
                            Regex.IsMatch(current, @"\b(experience)\b") &&
                            (Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b")) &&
                            !Regex.IsMatch(current, @"\b(volunteer)\b")
                            )
                        {
                            startIndex = i + 1;
                        }
                    }
                    else
                    {
                        if(
                            Regex.IsMatch(current, @".*(?=(\sat))") || Regex.IsMatch(current, @"(?<=(at\s)).*")
                            )
                        {
                            startIndex = i;
                            started = true;
                            input.Add(Lines[i]);
                        }
                        else if(
                            Regex.IsMatch(current, @"\b(experience)\b") &&
                            ((Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b")) ||
                            Regex.IsMatch(current, @"\b(volunteer)\b"))
                            )
                            break;
                    }
                }
                else
                {
                    CheckIfEnd(current, ref startIndex, ref started, input, experience, ref i);
                }
            }
            return experience.ToArray();
        }
        /// <summary>
        /// Gets all Volunteer Experience from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>An array of each experience entry</returns>
        private Applicant.Experience[] GetVolunteerExperience()
        {
            string current;
            int startIndex = -1;
            bool started = false;
            List<string> input = new List<string>();
            List<Applicant.Experience> experience = new List<Applicant.Experience>();
            for(int i = 0; i < Lines.Length; i++)
            {
                current = delimitersNoSpace.Replace(Lines[i].ToLower().Trim(), "");
                if(startIndex == -1)
                {
                    if(experience.Count == 0)
                    {
                        if(
                            Regex.IsMatch(current, @"\b(experience)\b") &&
                            (!Regex.IsMatch(current, @"\b(work)\b") || !Regex.IsMatch(current, @"\b(technical)\b")) &&
                            Regex.IsMatch(current, @"\b(volunteer)\b")
                            )
                        {
                            startIndex = i + 1;
                        }
                    }
                    else
                    {
                        if(
                            Regex.IsMatch(current, @".*(?=(\sat))") || Regex.IsMatch(current, @"(?<=(at\s)).*")
                            )
                        {
                            startIndex = i;
                            started = true;
                            input.Add(Lines[i]);
                        }
                        else if(
                            Regex.IsMatch(current, @"\b(experience)\b") &&
                            ((Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b")) ||
                            Regex.IsMatch(current, @"\b(volunteer)\b"))
                            )
                            break;
                    }
                }
                else
                {
                    CheckIfEnd(current, ref startIndex, ref started, input, experience, ref i);
                }
            }
            return experience.ToArray();
        }
        private void CheckIfEnd(string current, ref int startIndex, ref bool started, List<string> input, List<Applicant.Experience> experience, ref int i)
        {
            if(started)
            {
                if(
                    ((Regex.IsMatch(current, @".*(?=(\sat))") || Regex.IsMatch(current, @"(?<=(at\s)).*")) ||
                    (Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b"))) ||
                    Regex.IsMatch(current, @"\b(volunteer)\b")
                    )
                {
                    if(
                        !Regex.IsMatch(current, @"\b(pay)\b") ||
                        (Regex.IsMatch(current, @"\b(volunteer)\b") || Regex.IsMatch(current, @"\b(experience)\b")) ||
                        (Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b"))
                        )
                    {
                        started = false;
                        startIndex = -1;
                        experience.Add(Applicant.GetExperience(input.ToArray()));
                        input.Clear();
                        i--;
                    }
                    else
                        input.Add(Lines[i]);
                }
                else
                    input.Add(Lines[i]);
            }
            else
            {
                started = true;
                input.Add(Lines[i]);
            }
        }
        /// <summary>
        /// Gets all Educational Experience from the text of the applicant's resume and adds it to the applicant object
        /// </summary>
        /// <returns>An array of each entry</returns>
        private string[] GetEducationalExperience()
        {
            string current;
            int startIndex = -1;
            bool started = false;
            List<string> experience = new List<string>();
            for(int i = 0; i < Lines.Length; i++)
            {
                current = delimitersNoSpace.Replace(Lines[i].ToLower().Trim(), "");
                if(startIndex == -1)
                {
                    if(
                        ((!Regex.IsMatch(current, @"\b(work)\b") || !Regex.IsMatch(current, @"\b(technical)\b")) &&
                        !Regex.IsMatch(current, @"\b(volunteer)\b")) &&

                        (
                            Regex.IsMatch(current, @"\b(education)\b") ||
                            (Regex.IsMatch(current, @"\b(educational)\b") && Regex.IsMatch(current, @"\b(experience)\b"))
                        )
                        )
                    {
                        startIndex = i + 1;
                    }
                }
                else
                {
                    CheckIfEnd(current, ref startIndex, ref started, experience, ref i);
                }
            }
            return experience.ToArray();
        }
        private void CheckIfEnd(string current, ref int startIndex, ref bool started, List<string> experience, ref int i)
        {
            if(started)
            {
                if(
                    ((Regex.IsMatch(current, @".*(?=(\sat))") || Regex.IsMatch(current, @"(?<=(at\s)).*")) ||
                    ((Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b"))) ||
                    Regex.IsMatch(current, @"\b(volunteer)\b")) || Regex.IsMatch(current, @"\b(skills)\b")
                    )
                {
                    if(
                        !Regex.IsMatch(current, @"\b(pay)\b") ||
                        (Regex.IsMatch(current, @"\b(volunteer)\b") || Regex.IsMatch(current, @"\b(experience)\b")) ||
                        (Regex.IsMatch(current, @"\b(work)\b") || Regex.IsMatch(current, @"\b(technical)\b"))
                        )
                    {
                        started = false;
                        startIndex = -1;
                        experience.ToArray();
                    }
                    else
                        experience.Add(Lines[i]);
                }
                else
                    experience.Add(Lines[i]);
            }
            else
            {
                started = true;
                experience.Add(Lines[i]);
            }
        }
    }
}
