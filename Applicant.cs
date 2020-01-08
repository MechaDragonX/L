using System;
using System.Collections.Generic;
using System.Text;

namespace L
{
    public class Applicant
    {
        protected class Experience
        {
            protected string Title { get; set; }
            protected string Location { get; set; }
            protected DateTime StartDate { get; set; }
            protected DateTime EndDate { get; set; }
            protected string[] Responsibilities { get; set; }
            protected bool Paid { get; set; }

            protected Experience(string title)
            {
                Title = title;
            }
            protected Experience(string title, string location) : this(title)
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
        protected Experience[] EducationalXP { get; set; }
        protected Experience[] WorkXP { get; set; }
        protected Experience[] VolunteerXP { get; }
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
