using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? Age { get; set; }

        public Person(string name, string surname, string email, 
            int? phone, DateTime? dateadded, int? age)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
            DateAdded = dateadded;
            Age = age;
        }
    }
}