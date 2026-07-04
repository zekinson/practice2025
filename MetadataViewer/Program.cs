using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MetadataViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            string dllPath = args[0];

            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"Файл '{dllPath}' не найден");
                return;
            }

            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                Console.WriteLine($"Библиотека: {assembly.GetName().Name}\n");

                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.Namespace != null && !t.Namespace.StartsWith("System"))
                    .Where(t => !t.Name.Contains("<>"))
                    .Where(t => t.Name != "EmbeddedAttribute")
                    .Where(t => t.Name != "NullableAttribute")
                    .Where(t => t.Name != "NullableContextAttribute")
                    .Where(t => t.Name != "RefSafetyRulesAttribute")
                    .OrderBy(t => t.Name);

                foreach (var type in types)
                {
                    Console.WriteLine($"Класс: {type.Name}");

                    var classAttrs = type.GetCustomAttributes();
                    if (classAttrs.Any())
                    {
                        Console.WriteLine("Атрибуты:");
                        foreach (var attr in classAttrs)
                        {
                            string attrName = attr.GetType().Name;
                            if (attrName != "NullableContextAttribute" &&
                                attrName != "NullableAttribute" &&
                                attrName != "CompilerGeneratedAttribute" &&
                                attrName != "EmbeddedAttribute")
                            {
                                Console.WriteLine($"{attrName}");
                            }
                        }
                    }

                    var constructors = type.GetConstructors();
                    if (constructors.Any())
                    {
                        Console.WriteLine("Конструкторы:");
                        foreach (var ctor in constructors)
                        {
                            var params_ = ctor.GetParameters();
                            string paramStr = params_.Any() 
                                ? string.Join(", ", params_.Select(p => $"{p.ParameterType.Name} {p.Name}"))
                                : "нет параметров";
                            Console.WriteLine($"{type.Name}({paramStr})");
                        }
                    }

                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                        .Where(m => !m.IsSpecialName && m.DeclaringType == type)
                        .OrderBy(m => m.Name);
                    
                    if (methods.Any())
                    {
                        Console.WriteLine("Методы:");
                        foreach (var method in methods)
                        {
                            var params_ = method.GetParameters();
                            string paramStr = params_.Any() 
                                ? string.Join(", ", params_.Select(p => $"{p.ParameterType.Name} {p.Name}"))
                                : "нет параметров";
                            Console.WriteLine($"{method.ReturnType.Name} {method.Name}({paramStr})");
                        }
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}