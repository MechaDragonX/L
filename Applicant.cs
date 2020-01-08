using System;
using System.Collections.Generic;
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
        public Experience[] VolunteerXP { get; }
        protected string[] Technical { get; set; }
        protected string[] Unsorted { get; set; }

        public Applicant(string givenName, string surname, string email, string phoneNumber, string address, string summary)
        {
            GivenName = givenName;
            Surname = surname;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Summary = summary;
        }
    }
}
