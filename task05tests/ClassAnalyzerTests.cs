using System;
using Xunit;
using task05;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    
    public int Property { get; set; }
    private int PrivateProperty { get; set; }

    public void Method() { }
    public int MethodWithParams(int a, string b) => a;
    private void PrivateMethod() { }
}

public class EmptyTestClass {}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
        Assert.Contains("MethodWithParams", methods);
        Assert.DoesNotContain("PrivateMethod", methods);
    }

    [Fact]
    public void GetPublicMethods_WithEmptyClass_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(EmptyTestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Empty(methods);
    }

    [Fact]
    public void GetMethodParams_WithMethodWithParams_ReturnsParamsAndReturnType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var result = analyzer.GetMethodParams("MethodWithParams").ToList();

        Assert.Equal(3, result.Count);
        Assert.Contains("Int32 a", result);
        Assert.Contains("String b", result);
        Assert.Contains("Return: Int32", result);
    }

    [Fact]
    public void GetMethodParams_WithMethodWithoutParams_ReturnsOnlyReturnType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var result = analyzer.GetMethodParams("Method").ToList();

        Assert.Equal(1, result.Count);
        Assert.Contains("Return: Void", result);
    }

    [Fact]
    public void GetMethodParams_WithNoMethod_ReturnsEmpty()
{
    var analyzer = new ClassAnalyzer(typeof(TestClass));
    var result = analyzer.GetMethodParams("NoMethod");

    Assert.Empty(result);
}


    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetAllFields_IncludesAllFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields().ToList();

        Assert.Equal(2, fields.Count);
        Assert.Contains("PublicField", fields);
        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetAllFields_EmptyClass_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(EmptyTestClass));
        var fields = analyzer.GetAllFields();
        Assert.Empty(fields);
    }

    [Fact]
    public void GetProperties_ReturnsAllProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties().ToList();

        Assert.Equal(2, properties.Count);
        Assert.Contains("Property", properties);
        Assert.Contains("PrivateProperty", properties);
    }

    [Fact]
    public void GetProperties_WithEmptyClass_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(EmptyTestClass));
        var properties = analyzer.GetProperties();

        Assert.Empty(properties);
    }

    [Fact]
    public void HasAttribute_WithAttribute_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void HasAttribute_WitnNoAttribute_ReturnsFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
}