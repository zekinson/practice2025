using System;
using System.Threading;
using System.Threading.Tasks;

namespace task14
{
    public static class DefiniteIntegral
    {
        public static double Solve(
            double a, 
            double b, 
            Func<double, double> function, 
            double step, 
            int threadsNumber)
        {
            if (threadsNumber <= 0)
            {
                throw new ArgumentException("Количество потоков должно быть больше 0", nameof(threadsNumber));
            }
            
            if (step <= 0)
            {
                throw new ArgumentException("Шаг должен быть больше 0", nameof(step));
            }

            if (a >= b)
            {
                throw new ArgumentException("Левая граница должна быть меньше правой");
            }

            int totalSteps = (int)Math.Ceiling((b - a) / step);
            double actualStep = (b - a) / totalSteps;

            double sum = 0.0;

            int stepsPerThread = totalSteps / threadsNumber;
            int remainingSteps = totalSteps % threadsNumber;
            var barrier = new Barrier(threadsNumber);

            var threads = new Thread[threadsNumber];

            for (int t = 0; t < threadsNumber; t++)
            {
                int threadIndex = t;
                int startStep = threadIndex * stepsPerThread + Math.Min(threadIndex, remainingSteps);
                int endStep = startStep + stepsPerThread + (threadIndex < remainingSteps ? 1 : 0);

                threads[t] = new Thread(() =>
                {
                    double localSum = 0.0;
                    double xStart = a + startStep * actualStep;
                    int localSteps = endStep - startStep;

                    for (int i = 0; i < localSteps; i++)
                    {
                        double x1 = xStart + i * actualStep;
                        double x2 = xStart + (i + 1) * actualStep;
                        localSum += (function(x1) + function(x2)) * actualStep / 2.0;
                    }

                    InterlockedAdd(ref sum, localSum);

                    barrier.SignalAndWait();
                });

                threads[t].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return sum;
        }

        public static double SolveSingleThread(
            double a,
            double b,
            Func<double, double> function,
            double step)
        {
            if (step <= 0)
                throw new ArgumentException("Шаг должен быть > 0", nameof(step));

            if (a >= b)
                throw new ArgumentException("Левая граница должна быть меньше правой");

            int totalSteps = (int)Math.Ceiling((b - a) / step);
            double actualStep = (b - a) / totalSteps;

            double sum = 0.0;

            for (int i = 0; i < totalSteps; i++)
            {
                double x1 = a + i * actualStep;
                double x2 = a + (i + 1) * actualStep;
                sum += (function(x1) + function(x2)) * actualStep / 2.0;
            }

            return sum;
        }

        private static void InterlockedAdd(ref double location, double value)
        {
            double newValue, currentValue;
            do
            {
                currentValue = location;
                newValue = currentValue + value;
            } 
            while (Interlocked.CompareExchange(ref location, newValue, currentValue) != currentValue);
        }
    }
}