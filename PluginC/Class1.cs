using System;
using PluginLib;

namespace PluginC
{
    [PluginLoad("PluginC", "PluginA", "PluginB")]
    public class PluginC
    {
        public void Execute()
        {
            Console.WriteLine("PluginC: Выполнение... (зависит от PluginA и PluginB)");
        }
    }
}
