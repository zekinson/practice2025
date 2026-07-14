using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using PluginLib;
using PluginA;
using PluginB;
using PluginC;

namespace task10tests
{
    public class PluginTests
    {
        private string GetPluginsDirectory()
        {
            return Path.GetDirectoryName(typeof(PluginA.PluginA).Assembly.Location);
        }

        [Fact]
        public void PluginLoader_LoadsAllPlugins()
        {
            var loader = new PluginLoader.PluginLoader();
            var dir = GetPluginsDirectory();

            var plugins = loader.LoadPlugins(dir);
            var names = plugins.Select(p => p.GetType().Name).ToList();

            Assert.Equal(3, names.Count);
            Assert.Contains("PluginA", names);
            Assert.Contains("PluginB", names);
            Assert.Contains("PluginC", names);
        }

        [Fact]
        public void PluginLoader_LoadsPluginsInCorrectOrder()
        {
            var loader = new PluginLoader.PluginLoader();
            var dir = GetPluginsDirectory();

            var plugins = loader.LoadPlugins(dir);
            var names = plugins.Select(p => p.GetType().Name).ToList();

            var indexA = names.IndexOf("PluginA");
            var indexB = names.IndexOf("PluginB");
            var indexC = names.IndexOf("PluginC");

            Assert.True(indexA < indexB, "PluginA должен быть перед PluginB");
            Assert.True(indexB < indexC, "PluginB должен быть перед PluginC");
        }

        [Fact]
        public void PluginLoader_ExecutesAllPlugins()
        {
            // Arrange
            var loader = new PluginLoader.PluginLoader();
            var dir = GetPluginsDirectory();

            // Act
            var plugins = loader.LoadPlugins(dir);

            // Assert
            foreach (var plugin in plugins)
            {
                var exception = Record.Exception(() => plugin.Execute());
                Assert.Null(exception);
            }
        }
    }
}