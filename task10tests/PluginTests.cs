using System;
using System.IO;
using System.Linq;
using Xunit;
using PluginLib;
using PluginA;
using PluginB;
using PluginC;

namespace task10tests
{
    public class PluginTests
    {
        [Fact]
        public void PluginLoader_LoadsAllPluginsInCorrectOrder()
        {
            var loader = new PluginLoader.PluginLoader();
            var dir = Path.GetDirectoryName(typeof(PluginA.PluginA).Assembly.Location);

            var plugins = loader.LoadPlugins(dir);
            var names = plugins.Select(p => p.GetType().Name).ToList();

            Assert.Contains("PluginA", names);
            Assert.Contains("PluginB", names);
            Assert.Contains("PluginC", names);

            var indexA = names.IndexOf("PluginA");
            var indexB = names.IndexOf("PluginB");
            var indexC = names.IndexOf("PluginC");

            Assert.True(indexA < indexB, "PluginA должен быть перед PluginB");
            Assert.True(indexB < indexC, "PluginB должен быть перед PluginC");
        }
    }
}