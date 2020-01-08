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
        public string GivenName { get; }
        public string Surname { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string Address { get; }
        public string Summary { get; }

        // Experience, Skills, Abilities
        public string HighSchool { get; }
        public string CollegeUG { get; }
        public string CollegePG { get; }
        protected Experience[] EducationalXP { get; }
        protected Experience[] WorkXP { get; }
        protected Experience[] VolunteerXP { get; }
        protected string[] Technical { get; }
        protected string[] Unsorted { get; }

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
