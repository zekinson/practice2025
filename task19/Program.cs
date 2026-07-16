using System;
using System.IO;
using System.Text;
using System.Threading;
using task17;
using task18;

namespace task19
{
    class Program
    {
        private static readonly StringBuilder _log = new StringBuilder();

        static void Main(string[] args)
        {
            var scheduler = new RoundRobinScheduler();
            var server = new task18.ServerThread(scheduler);

            Console.WriteLine("Создание 5 команд");
            Console.WriteLine();
            _log.AppendLine("Создание 5 команд");
            _log.AppendLine();

            var commands = new List<TestCommand>();
            for (int i = 0; i < 4; i++)
            {
                var testCmd = new TestCommand(i + 1, 3);
                commands.Add(testCmd);
                scheduler.Add(new CommandAdapter(testCmd));
            }
            var testCmd5 = new TestCommand(5, 10);
            commands.Add(testCmd5);
            scheduler.Add(new CommandAdapter(testCmd5));

            server.Start();

            Console.WriteLine("Выполнение команд");
            Console.WriteLine();
            _log.AppendLine("Выполнение команд");
            _log.AppendLine();

            bool allHaveThreeCalls = false;
            while (!allHaveThreeCalls)
            {
                allHaveThreeCalls = true;
                foreach (var c in commands)
                {
                    if (!c.IsCompleted && c.Counter < 3)
                    {
                        allHaveThreeCalls = false;
                        break;
                    }
                }

                if (allHaveThreeCalls)
                {
                    Console.WriteLine("HardStop");
                    _log.AppendLine("HardStop\n");
                    server.Stop();
                    server.Join();
                    break;
                }

                Thread.Sleep(1);
            }

            Console.WriteLine();
            Console.WriteLine("Результат:");
            _log.AppendLine();
            _log.AppendLine("Результат:");

            foreach (var c in commands)
            {
                bool isFinished = c.IsCompleted || c.Counter >= c.MaxCalls;
                string status = isFinished ? "завершён" : "прерван";
                Console.WriteLine($"Поток {c.Id}: {c.Counter} вызовов ({status})");
                _log.AppendLine($"Поток {c.Id}: {c.Counter} вызовов ({status})");
            }

            SaveResults();
        }

        private static void SaveResults()
        {
            try
            {
                string filePath = "task19_results.txt";
                File.WriteAllText(filePath, _log.ToString());
                Console.WriteLine($"\nРезультаты сохранены в {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка сохранения: {ex.Message}");
            }
        }

        public static void AppendLog(string text)
        {
            lock (_log)
            {
                _log.Append(text);
            }
        }
    }

    public class CommandAdapter : ILongCommand
    {
        private readonly ICommand _command;
        private readonly TestCommand _testCommand;

        public bool IsCompleted => _testCommand?.IsCompleted ?? true;

        public CommandAdapter(ICommand command)
        {
            _command = command;
            _testCommand = command as TestCommand;
        }

        public void Execute()
        {
            if (_testCommand != null)
            {
                Program.AppendLog($"Поток {_testCommand.Id} -> {_testCommand.Counter}\n");
            }

            _command.Execute();

            if (_testCommand != null && _testCommand.IsCompleted)
            {
                Console.WriteLine($"Поток {_testCommand.Id} завершён");
                Program.AppendLog($"Поток {_testCommand.Id} завершён\n");
            }
        }
    }
}