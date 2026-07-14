using System;
using PluginLib;

namespace PluginB
{
    [PluginLoad("PluginB", "PluginA")]
    public class PluginB
    {
        public void Execute()
        {
            Console.WriteLine("PluginB");
        }
    }
}
