using System;

namespace task17
{
    public class TestCommand : ICommand
    {
        private readonly string _name;
        private readonly Action _action;

        public TestCommand(string name, Action action = null)
        {
            _name = name;
            _action = action ?? (() => { });
        }

        public void Execute()
        {
            _action?.Invoke();
        }
    }
}