using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using task17;

namespace task17tests
{
    public class ServerThreadTests
    {
        [Fact]
        public void HardStop_ShouldStopImmediately_AndNotExecuteRemainingCommands()
        {
            var server = new ServerThread();
            var executedCommands = new List<string>();

            server.Start();

            server.AddCommand(new TestCommand("1", () => executedCommands.Add("1")));
            server.AddCommand(new TestCommand("2", () => executedCommands.Add("2")));
            server.AddCommand(new HardStopCommand(server));
            server.AddCommand(new TestCommand("3", () => executedCommands.Add("3")));

            server.Join();

            Assert.False(server.IsRunning);
            Assert.Contains("1", executedCommands);
            Assert.Contains("2", executedCommands);
            Assert.DoesNotContain("3", executedCommands);
        }

        [Fact]
        public void AddCommand_AfterHardStop_ShouldThrowException()
        {
            var server = new ServerThread();
            server.Start();

            server.AddCommand(new HardStopCommand(server));
            server.Join();

            Assert.Throws<InvalidOperationException>(() =>
                server.AddCommand(new TestCommand("after", () => { })));
        }

        [Fact]
        public void SoftStop_ShouldExecuteAllCommandsBeforeStop()
        {
            var server = new ServerThread();
            var executedCommands = new List<string>();

            server.Start();

            server.AddCommand(new TestCommand("1", () => executedCommands.Add("1")));
            server.AddCommand(new TestCommand("2", () => executedCommands.Add("2")));
            server.AddCommand(new TestCommand("3", () => executedCommands.Add("3")));
            server.AddCommand(new SoftStopCommand(server));

            server.Join();

            Assert.False(server.IsRunning);
            Assert.Contains("1", executedCommands);
            Assert.Contains("2", executedCommands);
            Assert.Contains("3", executedCommands);
        }
        
        [Fact]
        public void SoftStop_ShouldNotExecuteCommandsAddedAfterStop()
        {
            var server = new ServerThread();
            var executedCommands = new List<string>();

            server.Start();

            server.AddCommand(new TestCommand("1", () => executedCommands.Add("1")));
            server.AddCommand(new TestCommand("2", () => executedCommands.Add("2")));
            server.AddCommand(new SoftStopCommand(server));

            // Ждём полного завершения потока
            server.Join();

            // Теперь поток точно остановлен
            Assert.Throws<InvalidOperationException>(() =>
                server.AddCommand(new TestCommand("3", () => executedCommands.Add("3"))));

            Assert.False(server.IsRunning);
            Assert.Contains("1", executedCommands);
            Assert.Contains("2", executedCommands);
            Assert.DoesNotContain("3", executedCommands);
        }

        [Fact]
        public void AddCommand_AfterSoftStop_ShouldThrowException()
        {
            var server = new ServerThread();
            server.Start();

            server.AddCommand(new SoftStopCommand(server));
            server.Join();

            Assert.Throws<InvalidOperationException>(() =>
                server.AddCommand(new TestCommand("after", () => { })));
        }
    }
}