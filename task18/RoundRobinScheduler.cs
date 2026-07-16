using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace task18
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly ConcurrentQueue<ILongCommand> _queue;
        private readonly List<ILongCommand> _activeCommands;
        private int _currentIndex;
        private readonly object _lock = new object();

        public RoundRobinScheduler()
        {
            _queue = new ConcurrentQueue<ILongCommand>();
            _activeCommands = new List<ILongCommand>();
            _currentIndex = 0;
        }

        public bool HasCommand()
        {
            lock (_lock)
            {
                if (!_queue.IsEmpty)
                {
                    return true;
                }
                foreach (var cmd in _activeCommands)
                {
                    if (!cmd.IsCompleted)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Add(ILongCommand cmd)
        {
            if (cmd == null)
            {
                throw new System.ArgumentNullException(nameof(cmd));
            }
            
            _queue.Enqueue(cmd);
        }

        public ILongCommand Select()
        {
            lock (_lock)
            {
                _activeCommands.RemoveAll(c => c.IsCompleted);

                while (_queue.TryDequeue(out var cmd))
                {
                    _activeCommands.Add(cmd);
                }

                if (_activeCommands.Count == 0)
                {
                    return null;
                }

                if (_currentIndex >= _activeCommands.Count)
                {
                    _currentIndex = 0;
                }

                int attempts = 0;
                while (attempts < _activeCommands.Count)
                {
                    var cmd = _activeCommands[_currentIndex];
                    _currentIndex = (_currentIndex + 1) % _activeCommands.Count;

                    if (!cmd.IsCompleted)
                    {
                        return cmd;
                    }

                    attempts++;
                }

                _activeCommands.Clear();
                return null;
            }
        }

        public int ActiveCount
        {
            get { lock (_lock) return _activeCommands.Count; }
        }

        public int QueueCount
        {
            get { return _queue.Count; }
        }
    }
}