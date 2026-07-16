using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using ScottPlot;
using task18;

namespace task18tests
{
    public class QuantumPerformanceTests
    {
        private const int CommandsCount = 10;
        private const int StepsPerCommand = 20;

        [Fact]
        public void MeasureAndSaveQuantumPerformance()
        {
            var results = new List<(int QuantumMs, double TotalTimeMs)>();

            int[] quantums = { 1, 5, 10, 25, 50, 100, 200 };

            foreach (var quantum in quantums)
            {
                double totalTime = RunExperiment(quantum);
                results.Add((quantum, totalTime));
            }

            SaveResults(results);
            SavePlot(results);
        }

        private double RunExperiment(int quantumMs)
        {
            var scheduler = new RoundRobinScheduler();
            var server = new ServerThread(scheduler);

            for (int i = 0; i < CommandsCount; i++)
            {
                var cmd = new CounterCommand(StepsPerCommand, quantumMs);
                scheduler.Add(cmd);
            }

            server.Start();

            var sw = Stopwatch.StartNew();

            while (scheduler.HasCommand())
            {
                var cmd = scheduler.Select();
                if (cmd != null)
                {
                    cmd.Execute();
                }
                else
                {
                    Thread.Sleep(1);
                }
            }

            sw.Stop();

            server.Stop();
            server.Join();

            return sw.Elapsed.TotalMilliseconds;
        }

        private void SaveResults(List<(int QuantumMs, double TotalTimeMs)> results)
        {
            string content = $@"Результаты исследования:

Количество команд: {CommandsCount}
Шагов на команду: {StepsPerCommand}

Результаты:
";
            foreach (var r in results)
            {
                content += $"Квант {r.QuantumMs,3} мс | Время {r.TotalTimeMs,8:F2} мс\n";
            }

            string filePath = "quantum_results.txt";
            File.WriteAllText(filePath, content);
        }

        private void SavePlot(List<(int QuantumMs, double TotalTimeMs)> results)
        {
            try
            {
                var plt = new ScottPlot.Plot();
                plt.Title("Зависимость времени выполнения от кванта");
                plt.XLabel("Квант времени (мс)");
                plt.YLabel("Время выполнения (мс)");

                var xs = results.Select(r => (double)r.QuantumMs).ToArray();
                var ys = results.Select(r => r.TotalTimeMs).ToArray();

                var scatter = plt.Add.Scatter(xs, ys);
                scatter.Label = "Время выполнения";

                plt.Add.Markers(xs, ys);

                plt.ShowLegend();
                plt.SavePng("quantum_performance.png", 800, 500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить график: {ex.Message}");
            }
        }
    }
}