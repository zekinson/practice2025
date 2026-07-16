using System;
using System.Threading;
using task17;

namespace task19
{
    public class TestCommand : ICommand
    {
        private readonly int _id;
        private int _counter;
        private readonly int _maxCalls;
        private bool _isCompleted;

        public int Id => _id;
        public int Counter => _counter;
        public int MaxCalls => _maxCalls;
        public bool IsCompleted => _isCompleted;

        public TestCommand(int id, int maxCalls = 3)
        {
            _id = id;
            _maxCalls = maxCalls;
            _counter = 0;
            _isCompleted = false;
        }

        public void Execute()
        {
            if (_isCompleted)
                return;

            _counter++;
            Console.WriteLine($"Поток {_id} вызов {_counter}");
            
            Thread.Sleep(10);

            if (_counter >= _maxCalls)
            {
                _isCompleted = true;
            }
        }
    }
}