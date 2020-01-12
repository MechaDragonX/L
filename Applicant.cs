using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace L
{
    public class Applicant
    {
        public class Experience
        {
            public string Title { get; set; }
            public string Location { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string[] Responsibilities { get; set; }
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
            return JsonConvert.SerializeObject(this);
        }
    }
}
