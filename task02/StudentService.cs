using System;
using System.Collections.Generic;
using System.Linq;

namespace task02
{
    public class StudentService
    {
        private readonly List<Student> _students;

        public StudentService(List<Student> students) => _students = students;

        // 1. Возвращает студентов указанного факультета
        public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        {
            return _students.Where(s => s.Faculty == faculty);
        }


        // 2. Возвращает студентов со средним баллом >= minAverageGrade
        public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
        {
            return from student in _students
                    where student.Grades.Average() >= minAverageGrade
                    select student;
        }

        // 3. Возвращает студентов, отсортированных по имени (A-Z)
        public IEnumerable<Student> GetStudentsOrderedByName()
        {
            return from student in _students
                    orderby student.Name
                    select student;
        }

        // 4. Группировка по факультету
        public ILookup<string, Student> GroupStudentsByFaculty()
        {
            return _students.ToLookup(s => s.Faculty);
        }

        // 5. Находит факультет с максимальным средним баллом
        public string GetFacultyWithHighestAverageGrade()
        {
            return _students.GroupBy(s => s.Faculty).Select(g => new
                {
                    Faculty = g.Key,
                    Average = g.Average(s => s.Grades.Any() ? s.Grades.Average() : 0)
                }).OrderByDescending(f => f.Average).FirstOrDefault().Faculty;
        }
    }
}