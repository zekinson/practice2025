using System;
using System.Threading;
using task17;

namespace task18
{
    public class ServerThread : task17.ServerThread
    {
        private readonly IScheduler _scheduler;
        private readonly Thread _thread;
        private volatile bool _isRunning;
        private readonly int _idleTimeout = 100;

        public bool IsRunning => _isRunning;

        public ServerThread(IScheduler scheduler) : base()
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _isRunning = false;

            _thread = new Thread(ProcessCommands);
            _thread.IsBackground = true;
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _thread.Start();
        }

        public void AddCommand(ILongCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!_isRunning)
                throw new InvalidOperationException("Поток не запущен");

            _scheduler.Add(command);
            _thread.Interrupt();
        }

        public void Stop()
        {
            _isRunning = false;
            _thread.Interrupt();
        }

        public new void Join()
        {
            _thread.Join();
        }

        private void ProcessCommands()
        {
            while (_isRunning)
            {
                try
                {
                    var command = _scheduler.Select();

                    if (command != null)
                    {
                        command.Execute();
                    }
                    else
                    {
                        Thread.Sleep(_idleTimeout);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}