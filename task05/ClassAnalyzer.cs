using System;
using System.Reflection;
using System.Collections.Generic;

namespace task05
{
    public class ClassAnalyzer
    {
        private Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods()
        {
            return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(m => !m.IsSpecialName).Select(m => m.Name).Distinct();
        }

        public IEnumerable<string> GetMethodParams(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                return Enumerable.Empty<string>();
            }
            var method = _type.GetMethod(methodName);

            if (method == null)
                return Enumerable.Empty<string>();

            return method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}");
        }

        public IEnumerable<string> GetAllFields()
        {
            return _type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | 
                           BindingFlags.Instance | BindingFlags.Static)
                .Select(f => f.Name);
        }

        public IEnumerable<string> GetProperties()
        {
            return _type
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                               BindingFlags.Instance | BindingFlags.Static)
                .Select(p => p.Name);
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return _type.IsDefined(typeof(T), inherit: true);
        }
    }
}