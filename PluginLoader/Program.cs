using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginLib;

namespace PluginLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string pluginsDir = args.Length > 0 ? args[0] : ".";

            if (!Directory.Exists(pluginsDir))
            {
                Console.WriteLine($"Папка '{pluginsDir}' не найдена");
                return;
            }

            var loader = new PluginLoader();
            var plugins = loader.LoadPlugins(pluginsDir);

            Console.WriteLine($"Найдено плагинов: {plugins.Count}\n");

            foreach (var plugin in plugins)
            {
                Console.WriteLine($"Загрузка: {plugin.Name}");
                plugin.Execute();
                Console.WriteLine();
            }
        }
    }

    public class PluginLoader
    {
        public List<dynamic> LoadPlugins(string directory)
        {
            var pluginInfos = new List<(Type Type, PluginLoadAttribute Attr)>();
            var loadedPlugins = new Dictionary<string, dynamic>();

            var dllFiles = Directory.GetFiles(directory, "*.dll");

            foreach (var dll in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);

                    foreach (var type in assembly.GetTypes())
                    {
                        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                        if (attr != null)
                        {
                            pluginInfos.Add((type, attr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки {Path.GetFileName(dll)}: {ex.Message}");
                }
            }

            Console.WriteLine();

            var sorted = PluginsSort(pluginInfos);

            foreach (var (type, attr) in sorted)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    loadedPlugins[attr.Name] = instance;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания {attr.Name}: {ex.Message}");
                }
            }

            return loadedPlugins.Values.ToList();
        }

        private List<(Type, PluginLoadAttribute)> PluginsSort(
            List<(Type Type, PluginLoadAttribute Attr)> plugins)
        {
            var result = new List<(Type, PluginLoadAttribute)>();
            var visited = new HashSet<string>();
            var temp = new HashSet<string>();

            foreach (var item in plugins)
            {
                if (!visited.Contains(item.Attr.Name))
                {
                    Visit(item, plugins, visited, temp, result);
                }
            }

            return result;
        }

        private void Visit(
            (Type Type, PluginLoadAttribute Attr) item,
            List<(Type Type, PluginLoadAttribute Attr)> all,
            HashSet<string> visited,
            HashSet<string> temp,
            List<(Type, PluginLoadAttribute)> result)
        {
            string name = item.Attr.Name;

            if (temp.Contains(name))
                throw new InvalidOperationException($"Зацикливание {name}");

            if (visited.Contains(name))
                return;

            temp.Add(name);

            foreach (var dep in item.Attr.Dependencies)
            {
                var depItem = all.FirstOrDefault(p => p.Attr.Name == dep);
                if (depItem.Type != null && !visited.Contains(dep))
                {
                    Visit(depItem, all, visited, temp, result);
                }
            }

            temp.Remove(name);
            visited.Add(name);
            result.Add(item);
        }
    }
}
