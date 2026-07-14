using System;
using PluginLib;

namespace PluginA
{
    [PluginLoad("PluginA")]
    public class PluginA
    {
        public void Execute()
        {
            Console.WriteLine("PluginA");
        }
    }
}
