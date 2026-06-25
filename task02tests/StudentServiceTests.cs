using Xunit;
using task02;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5, 4} },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        Assert.Equal("Анна", result[0].Name);
        Assert.Equal("Иван", result[1].Name);
        Assert.Equal("Петр", result[2].Name);
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        double minAverage = 4.5;
        var result = _service.GetStudentsWithMinAverageGrade(minAverage).ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Grades.Average() >= minAverage));
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroup()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.Equal(2, result.Count);

        var FITGroup = result["ФИТ"];
        Assert.Equal(2, FITGroup.Count());
        Assert.Contains(FITGroup, s => s.Name == "Иван");
        Assert.Contains(FITGroup, s => s.Name == "Анна");

        var economGroup = result["Экономика"];
        Assert.Equal(1, economGroup.Count());
        Assert.Contains(economGroup, s => s.Name == "Петр");

    }
}