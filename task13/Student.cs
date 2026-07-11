using System;
using System.Collections.Generic;

namespace task13
{
    public class Subject
    {
        public string Name { get; set; }
        public int Grade { get; set; }

        public Subject() { }

        public Subject(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Subject> Grades { get; set; }

        public Student() { }

        public Student(string firstName, string lastName, DateTime birthDate, List<Subject> grades)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Grades = grades ?? new List<Subject>();
        }
    }
}