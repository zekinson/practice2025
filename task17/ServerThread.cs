using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public class ServerThread
    {
        private readonly Thread _thread;
        private readonly BlockingCollection<ICommand> _queue;
        private volatile bool _softStopRequested;
        private volatile bool _hardStopRequested;
        private volatile bool _isRunning;

        public bool IsRunning => _isRunning;

        public ServerThread()
        {
            _queue = new BlockingCollection<ICommand>(new ConcurrentQueue<ICommand>());
            _softStopRequested = false;
            _hardStopRequested = false;
            _isRunning = false;

            _thread = new Thread(ProcessCommands);
            _thread.IsBackground = true;
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _softStopRequested = false;
            _hardStopRequested = false;
            _isRunning = true;
            _thread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_hardStopRequested || _softStopRequested)
                throw new InvalidOperationException("Поток остановлен");

            _queue.Add(command);
        }

        public void HardStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("HardStop может быть вызван только из самого потока");

            _hardStopRequested = true;
            _softStopRequested = false;
            _isRunning = false;

            while (_queue.TryTake(out _)) { }
            _queue.CompleteAdding();
        }

        public void SoftStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("SoftStop может быть вызван только из самого потока");

            _softStopRequested = true;
        }

        public void Join()
        {
            _thread.Join();
        }

        private void ProcessCommands()
        {
            try
            {
                while (_isRunning && !_hardStopRequested)
                {
                    if (_softStopRequested && _queue.Count == 0)
                    {
                        _isRunning = false;
                        _queue.CompleteAdding();
                        break;
                    }

                    if (_queue.TryTake(out var command, 100))
                    {
                        try
                        {
                            command.Execute();
                        }
                        catch
                        {}
                    }
                }
            }
            finally
            {
                _isRunning = false;
                _queue.CompleteAdding();
            }
        }
    }
}