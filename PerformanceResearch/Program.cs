using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using task14;
using ScottPlot;

namespace PerformanceResearch
{
    class Program
    {
        private const double A = -100;
        private const double B = 100;
        
        private static readonly Func<double, double> Function = Math.Sin;
        
        private static readonly Func<double, double> StepSearchFunction = x => x * x;
        
        private const double Precision = 1e-4;
        private const int MeasurementsPerStep = 5;

        static void Main(string[] args)
        {
            Console.WriteLine($"Функция для замеров: sin(x)");
            Console.WriteLine($"Функция для поиска шага: x^2 (опр. интеграл sin(x) на отрезке [-100, 100] не подойдет)");
            Console.WriteLine($"Отрезок: [{A}, {B}]");
            Console.WriteLine($"Точность: {Precision}");
            Console.WriteLine();

            try
            {
                double optimalStep = FindOptimalStep();
                var (bestThreads, bestTime, singleTime) = FindOptimalThreads(optimalStep);
                PrintResults(optimalStep, bestThreads, singleTime, bestTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadKey();
        }

        static double FindOptimalStep()
        {
            double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
            var results = new List<(double Step, double Error)>();

            double exactValue = 2.0 * Math.Pow(100, 3) / 3.0;

            foreach (var step in steps)
            {
                double value = DefiniteIntegral.SolveSingleThread(A, B, StepSearchFunction, step);
                double error = Math.Abs(value - exactValue);
                results.Add((step, error));
            }

            double optimalStep = results.First(r => r.Error <= Precision).Step;
            Console.WriteLine($"Оптимальный шаг: {optimalStep:E1} (ошибка: {results.First(r => r.Step == optimalStep).Error:E4})");
            Console.WriteLine();

            return optimalStep;
        }

        static (int BestThreads, double BestTime, double SingleTime) FindOptimalThreads(double step)
        {
            Console.WriteLine($"Подбор оптимального количества потоков с шагом {step:E1}:\n");

            double singleTime = MeasureAverage(() =>
                DefiniteIntegral.SolveSingleThread(A, B, Function, step));

            Console.WriteLine($"Однопоточная версия: {singleTime:F2} мс");
            Console.WriteLine();

            int maxThreads = Environment.ProcessorCount * 2;
            var results = new List<(int Threads, double TimeMs)>();

            Console.WriteLine($"{"Потоков",8} {"Время, мс",12} {"Улучшение, %",14}");
            Console.WriteLine();

            for (int threads = 1; threads <= maxThreads; threads++)
            {
                double avgTime = MeasureAverage(() =>
                    DefiniteIntegral.Solve(A, B, Function, step, threads));

                double improvement = (singleTime - avgTime) / singleTime * 100;
                results.Add((threads, avgTime));

                Console.WriteLine($"{threads,8} {avgTime,12:F2} {improvement,14:F1}%");
            }

            var best = results.OrderBy(r => r.TimeMs).First();
            Console.WriteLine();
            Console.WriteLine($"Оптимальное количество потоков: {best.Threads}");
            Console.WriteLine($"Минимальное время: {best.TimeMs:F2} мс");
            Console.WriteLine();

            SavePlotToPng(results, singleTime);

            return (best.Threads, best.TimeMs, singleTime);
        }

        static double MeasureAverage(Action action)
        {
            action();

            double total = 0;
            for (int i = 0; i < MeasurementsPerStep; i++)
            {
                var sw = Stopwatch.StartNew();
                action();
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }

            return total / MeasurementsPerStep;
        }

        static void SavePlotToPng(List<(int Threads, double TimeMs)> results, double singleTime)
        {
            try
            {
                var plt = new ScottPlot.Plot();
                plt.Title("Производительность вычисления интеграла sin(x)");
                plt.XLabel("Количество потоков");
                plt.YLabel("Время выполнения (мс)");

                var xs = results.Select(r => (double)r.Threads).ToArray();
                var ys = results.Select(r => r.TimeMs).ToArray();

                plt.Add.Scatter(xs, ys);
                plt.Add.HorizontalLine(singleTime);

                string filePath = "performance_graph.png";
                plt.SavePng(filePath, 800, 500);
                Console.WriteLine($"График сохранён в {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось сохранить график: {ex.Message}");
            }
        }

        static void PrintResults(double step, int threads, double singleTime, double multiTime)
        {
            Console.WriteLine("Результат:");
            Console.WriteLine();

            double improvement = (singleTime - multiTime) / singleTime * 100;

            Console.WriteLine($"Оптимальный шаг:               {step:E1}");
            Console.WriteLine($"Оптимальное количество потоков: {threads}");
            Console.WriteLine($"Однопоточная версия:            {singleTime:F2} мс");
            Console.WriteLine($"Многопоточная версия:           {multiTime:F2} мс");
            Console.WriteLine($"Улучшение:                      {improvement:F1}%");

            SaveResults(step, threads, singleTime, multiTime, improvement);
        }

        static void SaveResults(double step, int threads, double singleTime, double multiTime, double improvement)
        {
            string content = $@"Результаты:

1. Функция для замеров: sin(x)
2. Функция для поиска шага: x^2
3. Отрезок: [{A}, {B}]
4. Точность: {Precision}

5. Оптимальный шаг: {step:E1}

6. Оптимальное количество потоков: {threads}

7. Сравнение производительности:
Однопоточная версия: {singleTime:F2} мс
Лучшая многопоточная версия: {multiTime:F2} мс
Улучшение: {improvement:F1}%
";

            string filePath = "performance_results.txt";
            File.WriteAllText(filePath, content);
            Console.WriteLine();
            Console.WriteLine($"Результаты сохранены в {filePath}");
        }
    }
}