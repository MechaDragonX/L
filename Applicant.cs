using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }

        // Experience, Skills, Abilities
        public string HighSchool { get; set; }
        public string CollegeUG { get; set; }
        public string CollegePG { get; set; }
        public Experience[] EducationalXP { get; set; }
        public Experience[] WorkXP { get; set; }
        public Experience[] VolunteerXP { get; set; }
        public string[] TechnicalSkills { get; set; }
        public string[] UnsortedSkills { get; set; }

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
    }
}
