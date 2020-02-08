using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace L
{
    public class Applicant
    {
        public class Experience
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("location")]
            public string Location { get; set; }
            [JsonProperty("startDate")]
            public DateTime StartDate { get; set; }
            [JsonProperty("endDate")]
            public DateTime EndDate { get; set; }
            [JsonProperty("responsibilities")]
            public string[] Responsibilities { get; set; }
            [JsonProperty("paid?")]
            public bool Paid { get; set; }

            public Experience() { }
            public Experience(string title)
            {
                Title = title;
            }
            public Experience(string title, string location) : this(title)
            {
                Location = location;
            }

            internal static string GetTitle(string input)
            {
                if(Regex.IsMatch(input, @".*(?=(\sat))"))
                    return Regex.Match(input, @".*(?=(\sat))").Value;
                return "";
            }
            internal static string GetLocation(string input)
            {
                if(Regex.IsMatch(input, @"(?<=(at\s)).*"))
                    return Regex.Match(input, @"(?<=(at\s)).*").Value;
                return "";
            }
            internal static DateTime GetStartDate(string input)
            {
                if(Regex.IsMatch(input, @".*(?=(\s[-–—]))"))
                {
                    if((Regex.Match(input, @".*(?=(\s[-–—]))").Value).Split(' ').Length == 1)
                        return DateTimeParser.Parse(
                            int.Parse(Regex.Match(input, @"\b(\d\d\d\d)\b").Value),
                            Regex.Match(input, @".*(?=(\s[-–—]))").Value
                            );
                    return DateTime.Parse(Regex.Match(input, @".*(?=(\s[-–—]))").Value);
                }
                return DateTime.MinValue;
            }
            internal static DateTime GetEndDate(string input)
            {
                string match;
                if(Regex.IsMatch(input, @"(?<=([-–—]\s)).*"))
                {
                    match = Regex.Match(input, @"(?<=([-–—]\s)).*").Value;
                    if(match.ToLower() == "present" || match.ToLower() == "now")
                        return DateTime.Today;
                    else
                        return DateTime.Parse(match);
                }
                return DateTime.MinValue;
            }
            internal static string[] GetResponsibilites(string[] input)
            {
                List<string> responsibilities = new List<string>();
                Regex beforeAt = new Regex(@".*(?=(\sat))", RegexOptions.Compiled);
                Regex afterAt = new Regex(@"(?<=(at\s)).*", RegexOptions.Compiled);
                Regex beforeDash = new Regex(@".*(?=(\s[-–—]))", RegexOptions.Compiled);
                Regex afterDash = new Regex(@"(?<=([-–—]\s)).*", RegexOptions.Compiled);

                foreach(string element in input)
                {
                    if(
                        !beforeAt.IsMatch(element) && !afterAt.IsMatch(element) &&
                        !beforeDash.IsMatch(element) && !afterDash.IsMatch(element)
                        )
                        responsibilities.Add(element);
                }
                return responsibilities.ToArray();
            }
            internal static bool GetPaid(string[] input)
            {
                foreach(string element in input)
                {
                    if(Regex.IsMatch(element, @"\b(pay)\b") && !Regex.IsMatch(element, @"\b(without)\b"))
                        return true;
                }
                return false;
            }

            /// <summary>
            /// String representation of an Applicant
            /// </summary>
            /// <returns>A JSON format string</returns>
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }

        [JsonProperty("id")]
        private int ID = 0;

        // Contact Info
        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        // Experience, Skills, and Abilities
        [JsonProperty("highSchool")]
        public string HighSchool { get; set; }

        [JsonProperty("collegeUG")]
        public string CollegeUG { get; set; }

        [JsonProperty("collegePG")]
        public string CollegePG { get; set; }

        [JsonProperty("educationalExperience")]
        public string[] EducationalXP { get; set; }

        [JsonProperty("workExperience")]
        public Experience[] WorkXP { get; set; }

        [JsonProperty("volunteerExperience")]
        public Experience[] VolunteerXP { get; set; }

        [JsonProperty("technicalSkills")]
        public string[] TechnicalSkills { get; set; }

        [JsonProperty("unsortedSkills")]
        public string[] UnsortedSkills { get; set; }

        public Applicant() { }
        public Applicant(string givenName, string surname)
        {
            GivenName = givenName;
            Surname = surname;
        }
        public Applicant(string givenName, string surname, string email) : this(givenName, surname)
        {
            Email = email;
        }
        public Applicant(string givenName, string surname, string email, string phoneNumber) : this(givenName, surname, email)
        {
            PhoneNumber = phoneNumber;
        }
        public Applicant(string givenName, string surname, string email, string phoneNumber, string address)
            : this(givenName, surname, email, phoneNumber)
        {
            Address = address;
        }
        public Applicant(string givenName, string surname, string email, string phoneNumber, string address, string summary)
            : this(givenName, surname, email, phoneNumber, address)
        {
            Summary = summary;
        }

        public void GenerateID()
        {
            foreach(char letter in Surname)
                ID += (letter * 3);
            foreach(char letter in GivenName)
                ID += (letter * 5);
        }
        public static Experience GetExperience(string[] input)
        {
            Regex beforeAt = new Regex(@".*(?=(\sat))", RegexOptions.Compiled);
            Regex afterAt = new Regex(@"(?<=(at\s)).*", RegexOptions.Compiled);
            Regex beforeDash = new Regex(@".*(?=(\s[-–—]))", RegexOptions.Compiled);
            Regex afterDash = new Regex(@"(?<=([-–—]\s)).*", RegexOptions.Compiled);

            string title = "";
            string location = "";
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            foreach(string element in input)
            {
                if(beforeAt.IsMatch(element) && title == "")
                    title = Experience.GetTitle(element);
                if(afterAt.IsMatch(element) && location == "")
                    location = Experience.GetLocation(element);
                if(beforeDash.IsMatch(element))
                    startDate = Experience.GetStartDate(element);
                if(afterDash.IsMatch(element))
                    endDate = Experience.GetEndDate(element);
            }

            Experience experience = new Experience(title, location)
            {
                StartDate = startDate,
                EndDate = endDate,
                Responsibilities = Experience.GetResponsibilites(input),
                Paid = Experience.GetPaid(input)
            };

            return experience;
        }
        public void Serialize(bool deploy)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            string directory = "";
            string yesNo;
            Console.WriteLine("Do you want to send the \".json\" file to a custom folder? (Y or N)");
            while(true)
            {
                yesNo = Console.ReadLine().Trim().ToLower();
                if(yesNo == "y")
                {
                    Console.WriteLine("Please type the path to the output directory.");
                    directory = Console.ReadLine();
                    break;
                }
                else if (yesNo == "n")
                {
                    if(!deploy)
                        directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    else
                        directory = Environment.CurrentDirectory;
                    break;
                }
                else
                    Console.WriteLine("Please type yes or no!\n");
            }

            string name = string.Format("{0}, {1}", Surname, GivenName);
            Directory.CreateDirectory(Path.Join(directory, "out"));
            using (StreamWriter sWriter = new StreamWriter(Path.Join(directory, "out", name + ".json")))
            using(JsonWriter jWriter = new JsonTextWriter(sWriter))
                serializer.Serialize(jWriter, this);
        }
        /// <summary>
        /// Deserialize and Applicant object from a JSON file
        /// </summary>
        /// <param name="path">Path to the JSON file</param>
        /// <returns>The deserialized object</returns>
        public static Applicant Deserialize(string path)
        {
            return JsonConvert.DeserializeObject<Applicant>(File.ReadAllText(path));
        }

        /// <summary>
        /// String representation of an Applicant
        /// </summary>
        /// <returns>A JSON format string</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
