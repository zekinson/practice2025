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
        private volatile bool _hardStopRequested;
        private readonly int _idleTimeout = 100;

        public bool IsRunning => _isRunning;

        public ServerThread(IScheduler scheduler) : base()
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _isRunning = false;
            _hardStopRequested = false;

            _thread = new Thread(ProcessCommands);
            _thread.IsBackground = true;
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _hardStopRequested = false;
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
            _hardStopRequested = true;
            _isRunning = false;
            _thread.Interrupt();
        }

        public new void Join()
        {
            _thread.Join();
        }

        private void ProcessCommands()
        {
            while (_isRunning && !_hardStopRequested)
            {
                try
                {
                    var command = _scheduler.Select();

                    if (command != null)
                    {
                        command.Execute();

                        if (_hardStopRequested)
                            break;
                    }
                    else
                    {
                        Thread.Sleep(_idleTimeout);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    if (_hardStopRequested)
                        break;
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            _isRunning = false;
        }
    }
}