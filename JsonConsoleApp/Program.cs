using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using task13;

namespace JsonConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var student = new Student
                {
                    FirstName = "Иван",
                    LastName = "Петров",
                    BirthDate = new DateTime(2000, 5, 15),
                    Grades = new List<Subject>
                    {
                        new Subject("Математика", 5),
                        new Subject("Физика", 4),
                        new Subject("Программирование", 5)
                    }
                };

                ValidateStudent(student);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string json = JsonSerializer.Serialize(student, options);
                Console.WriteLine("Сериализация в JSON:");
                Console.WriteLine(json);
                Console.WriteLine();

                string filePath = "student.json";
                File.WriteAllText(filePath, json);
                Console.WriteLine($"Сохранено в файл: {filePath}");
                Console.WriteLine();

                string jsonFromFile = File.ReadAllText(filePath);
                var loadedStudent = JsonSerializer.Deserialize<Student>(jsonFromFile, options);

                ValidateStudent(loadedStudent);
                Console.WriteLine("Загрузка из файла:");
                Console.WriteLine($"Имя: {loadedStudent.FirstName}");
                Console.WriteLine($"Фамилия: {loadedStudent.LastName}");
                Console.WriteLine($"Дата рождения: {loadedStudent.BirthDate:yyyy-MM-dd}");
                Console.WriteLine("Оценки:");
                foreach (var grade in loadedStudent.Grades)
                {
                    Console.WriteLine($"{grade.Name}: {grade.Grade}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ValidateStudent(Student student)
        {
            if (student == null)
                throw new InvalidOperationException("Студент не может быть null");

            if (string.IsNullOrEmpty(student.FirstName))
                throw new InvalidOperationException("Имя не может быть пустым");

            if (string.IsNullOrEmpty(student.LastName))
                throw new InvalidOperationException("Фамилия не может быть пустой");

            if (student.BirthDate == DateTime.MinValue)
                throw new InvalidOperationException("Дата рождения не указана");

            if (student.BirthDate > DateTime.Now)
                throw new InvalidOperationException("Дата рождения не может быть в будущем");

            if (student.Grades == null)
                throw new InvalidOperationException("Список оценок не может быть null");

            foreach (var grade in student.Grades)
            {
                if (string.IsNullOrEmpty(grade.Name))
                    throw new InvalidOperationException("Название предмета не может быть пустым");

                if (grade.Grade < 1 || grade.Grade > 5)
                    throw new InvalidOperationException($"Оценка '{grade.Grade}' должна быть в диапазоне 1-5");
            }
        }
    }
}