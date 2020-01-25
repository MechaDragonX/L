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
            [JsonProperty("Title")]
            public string Title { get; set; }
            [JsonProperty("Location")]
            public string Location { get; set; }
            [JsonProperty("Start Date")]
            public DateTime StartDate { get; set; }
            [JsonProperty("End Date")]
            public DateTime EndDate { get; set; }
            [JsonProperty("Responsibilities")]
            public string[] Responsibilities { get; set; }
            [JsonProperty("Paid?")]
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

        // Contact Info
        [JsonProperty("Given Name")]
        public string GivenName { get; set; }

        [JsonProperty("Surname")]
        public string Surname { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Phone Number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Summary")]
        public string Summary { get; set; }

        // Experience, Skills, and Abilities
        [JsonProperty("High School")]
        public string HighSchool { get; set; }

        [JsonProperty("College UG")]
        public string CollegeUG { get; set; }

        [JsonProperty("College PG")]
        public string CollegePG { get; set; }

        [JsonProperty("Educational Experience")]
        public Experience[] EducationalXP { get; set; }

        [JsonProperty("Work Experience")]
        public Experience[] WorkXP { get; set; }

        [JsonProperty("Volunteer Experience")]
        public Experience[] VolunteerXP { get; set; }

        [JsonProperty("Technical Skills")]
        public string[] TechnicalSkills { get; set; }

        [JsonProperty("Unsorted Skills")]
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

            foreach (string element in input)
            {
                if (beforeAt.IsMatch(element) && title == "")
                    title = Experience.GetTitle(element);
                if (afterAt.IsMatch(element) && location == "")
                    location = Experience.GetLocation(element);
                if (beforeDash.IsMatch(element))
                    startDate = Experience.GetStartDate(element);
                if (afterDash.IsMatch(element))
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
        public void Serialize()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string name = string.Format("{0}, {1}", Surname, GivenName);
            using(StreamWriter sWriter = new StreamWriter(Path.Join(projectDir, "out", name + ".json")))
            using(JsonWriter jWriter = new JsonTextWriter(sWriter))
            {
                serializer.Serialize(jWriter, this);
            }
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
