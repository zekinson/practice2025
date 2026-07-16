using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using task18;

namespace task18tests
{
    public class SchedulerTests
    {
        [Fact]
        public void Scheduler_ShouldExecuteAllCommands()
        {
            var scheduler = new RoundRobinScheduler();
            var commands = new List<CounterCommand>();

            for (int i = 0; i < 3; i++)
            {
                var cmd = new CounterCommand(3);
                commands.Add(cmd);
                scheduler.Add(cmd);
            }

            int executedCount = 0;
            while (scheduler.HasCommand())
            {
                var cmd = scheduler.Select();
                if (cmd != null)
                {
                    cmd.Execute();
                    executedCount++;
                }
            }

            Assert.Equal(9, executedCount);
            foreach (var cmd in commands)
            {
                Assert.True(cmd.IsCompleted);
            }
        }

        [Fact]
        public void Scheduler_ShouldUseRoundRobin()
        {
            var scheduler = new RoundRobinScheduler();
            var commands = new List<CounterCommand>();

            for (int i = 0; i < 3; i++)
            {
                var cmd = new CounterCommand(3);
                commands.Add(cmd);
                scheduler.Add(cmd);
            }

            int executedCount = 0;
            while (scheduler.HasCommand())
            {
                var cmd = scheduler.Select();
                if (cmd != null)
                {
                    cmd.Execute();
                    executedCount++;
                }
            }

            Assert.Equal(9, executedCount);
            foreach (var cmd in commands)
            {
                Assert.True(cmd.IsCompleted);
            }
        }

        [Fact]
        public void Scheduler_ShouldHandleNewCommandsDuringExecution()
        {
            var scheduler = new RoundRobinScheduler();
            var server = new ServerThread(scheduler);
            
            server.Start();

            var cmd1 = new CounterCommand(5);
            scheduler.Add(cmd1);

            Thread.Sleep(50);

            var cmd2 = new CounterCommand(3);
            scheduler.Add(cmd2);

            Thread.Sleep(200);

            server.Stop();
            server.Join();

            Assert.True(cmd1.IsCompleted);
            Assert.True(cmd2.IsCompleted);
        }

        [Fact]
        public void Scheduler_HasCommand_ReturnsCorrectState()
        {
            var scheduler = new RoundRobinScheduler();

            Assert.False(scheduler.HasCommand());

            var cmd = new CounterCommand(3);
            scheduler.Add(cmd);
            
            Assert.True(scheduler.HasCommand());

            while (scheduler.HasCommand())
            {
                var selected = scheduler.Select();
                selected?.Execute();
            }

            Assert.False(scheduler.HasCommand());
        }

        [Fact]
        public void ServerThread_ShouldNotUseCPUWhenIdle()
        {
            var scheduler = new RoundRobinScheduler();
            var server = new ServerThread(scheduler);

            server.Start();
            
            Thread.Sleep(500);
            
            server.Stop();
            server.Join();

            Assert.False(server.IsRunning);
        }
    }
}