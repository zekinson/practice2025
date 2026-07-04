using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;

namespace CommandRunner
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
                Console.WriteLine($"Загружена библиотека: {assembly.GetName().Name}");

                var commandTypes = assembly.GetTypes()
                    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                    .ToList();

                if (!commandTypes.Any())
                {
                    Console.WriteLine("Не найдено команд, реализующих ICommand");
                    return;
                }

                Console.WriteLine($"Найдено команд: {commandTypes.Count}\n");

                foreach (var type in commandTypes)
                {
                    Console.WriteLine($"Команда {type.Name}:");

                    try
                    {
                        object instance = CreateCommandInstance(type);

                        if (instance is ICommand command)
                        {
                            command.Execute();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
        }

        private static object CreateCommandInstance(Type type)
        {
            var constructors = type.GetConstructors();

            foreach (var constr in constructors)
            {
                var parameters = constr.GetParameters();
                object[] args = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    Console.Write($"Введите значение для {param.Name} ({param.ParameterType.Name}): ");
                    string input = Console.ReadLine();

                    if (param.ParameterType == typeof(string))
                    {
                        args[i] = input ?? "";
                    }
                    else if (param.ParameterType == typeof(int))
                    {
                        args[i] = int.TryParse(input, out int val) ? val : 0;
                    }
                    else
                    {
                        args[i] = Convert.ChangeType(input, param.ParameterType);
                    }
                }

                try
                {
                    return Activator.CreateInstance(type, args);
                }
                catch
                {
                    Console.WriteLine("Ошибка");
                    continue;
                }
            }
            return Activator.CreateInstance(type);
        }
    }
}