using System;
using System.Threading;

namespace task18
{
    public class CounterCommand : ILongCommand
    {
        private int _counter;
        private readonly int _maxCount;
        private readonly int _workQuantumMs;
        public bool IsCompleted => _counter >= _maxCount;

        public CounterCommand(int maxCount, int workQuantumMs = 10)
        {
            _maxCount = maxCount;
            _counter = 0;
            _workQuantumMs = workQuantumMs;
        }

        public void Execute()
        {
            if (IsCompleted)
            {
                return;
            }
            
            Thread.Sleep(_workQuantumMs);
            _counter++;
        }

        public int GetProgress() => _counter;
    }
}