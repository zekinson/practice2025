using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11
{
    public static class ClassGenerator
    {
        public static object CreateInstance(string classCode, string className)
        {
            string fullCode = @"
using System;

" + classCode;

            var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);

            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            };

            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new System.IO.MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics);
                throw new Exception($"Ошибка компиляции:\n{errors}");
            }

            ms.Seek(0, System.IO.SeekOrigin.Begin);

            var assembly = Assembly.Load(ms.ToArray());

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name == className && !type.IsInterface && !type.IsAbstract)
                {
                    return Activator.CreateInstance(type);
                }
            }

            throw new Exception($"Класс '{className}' не найден");
        }
    }
}