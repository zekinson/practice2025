using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using task13;

namespace task13tests
{
    public class JsonTests
    {
        private Student CreateTestStudent()
        {
            return new Student
            {
                FirstName = "Иван",
                LastName = "Петров",
                BirthDate = new DateTime(2000, 5, 15),
                Grades = new List<Subject>
                {
                    new Subject("Математика", 5),
                    new Subject("Физика", 4)
                }
            };
        }

        private JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        [Fact]
        public void Serialize_ValidStudent_ReturnsJson()
        {
            var student = CreateTestStudent();
            var options = GetOptions();

            var json = JsonSerializer.Serialize(student, options);

            Assert.Contains("Иван", json);
            Assert.Contains("Петров", json);
            Assert.Contains("2000-05-15", json);
            Assert.Contains("Математика", json);
            Assert.Contains("5", json);
        }

        [Fact]
        public void Deserialize_ValidJson_ReturnsStudent()
        {
            var json = @"
            {
                ""firstName"": ""Иван"",
                ""lastName"": ""Петров"",
                ""birthDate"": ""2000-05-15T00:00:00"",
                ""grades"": [
                    { ""name"": ""Математика"", ""grade"": 5 },
                    { ""name"": ""Физика"", ""grade"": 4 }
                ]
            }";

            var options = GetOptions();

            var student = JsonSerializer.Deserialize<Student>(json, options);

            Assert.Equal("Иван", student.FirstName);
            Assert.Equal("Петров", student.LastName);
            Assert.Equal(new DateTime(2000, 5, 15), student.BirthDate);
            Assert.Equal(2, student.Grades.Count);
            Assert.Equal("Математика", student.Grades[0].Name);
            Assert.Equal(5, student.Grades[0].Grade);
        }


        [Fact]
        public void Validate_ValidStudent_DoesNotThrow()
        {
            var student = CreateTestStudent();
            var exception = Record.Exception(() => ValidateStudent(student));
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_EmptyFirstName_ThrowsException()
        {
            var student = CreateTestStudent();
            student.FirstName = "";
            Assert.Throws<InvalidOperationException>(() => ValidateStudent(student));
        }

        [Fact]
        public void SerializeDeserialize_ReturnsSameObject()
        {
            var original = CreateTestStudent();
            var options = GetOptions();

            var json = JsonSerializer.Serialize(original, options);
            var restored = JsonSerializer.Deserialize<Student>(json, options);

            ValidateStudent(restored);

            Assert.Equal(original.FirstName, restored.FirstName);
            Assert.Equal(original.LastName, restored.LastName);
            Assert.Equal(original.BirthDate, restored.BirthDate);
            Assert.Equal(original.Grades.Count, restored.Grades.Count);
        }

        private void ValidateStudent(Student student)
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