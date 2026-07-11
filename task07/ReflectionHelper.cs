using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task07
{
    public static class ReflectionHelper
    {
        public static string PrintTypeInfo(Type type)
        {
            string result = "";

            var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayName != null)
                result += $"Отображаемое имя: {displayName.DisplayName}\n";

            var version = type.GetCustomAttribute<VersionAttribute>();
            if (version != null)
                result += $"Версия: {version}\n";

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            bool hasMarkedProperties = false;
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                {
                    if (!hasMarkedProperties)
                    {
                        result += "Свойства:\n";
                        hasMarkedProperties = true;
                    }
                    result += $"{prop.Name}: {attr.DisplayName}\n";
                }
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                              .Where(m => !m.IsSpecialName);
            bool hasMarkedMethods = false;
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                {
                    if (!hasMarkedMethods)
                    {
                        result += "Методы:\n";
                        hasMarkedMethods = true;
                    }
                    result += $"{method.Name}: {attr.DisplayName}\n";
                }
            }

            return result;
        }
    }
}