using System;
using Xunit;
using task19;

namespace task19tests
{
    public class CommandTests
    {
        [Fact]
        public void TestCommand_ShouldExecuteMaxCalls()
        {
            var cmd = new TestCommand(1, 3);
            
            cmd.Execute();
            cmd.Execute();
            cmd.Execute();
            
            Assert.True(cmd.IsCompleted);
            Assert.Equal(3, cmd.Counter);
        }

        [Fact]
        public void TestCommand_ShouldNotExecuteAfterCompleted()
        {
            var cmd = new TestCommand(1, 2);
            
            cmd.Execute();
            cmd.Execute();
            cmd.Execute();
            
            Assert.True(cmd.IsCompleted);
            Assert.Equal(2, cmd.Counter);
        }

        [Fact]
        public void TestCommand_ShouldHaveCorrectId()
        {
            var cmd = new TestCommand(5, 3);
            Assert.Equal(5, cmd.Id);
        }

        [Fact]
        public void TestCommand_ShouldHaveCorrectMaxCalls()
        {
            var cmd = new TestCommand(1, 10);
            Assert.Equal(10, cmd.MaxCalls);
        }

        [Fact]
        public void TestCommand_ShouldExecuteStepByStep()
        {
            var cmd = new TestCommand(1, 5);
            
            for (int i = 1; i <= 5; i++)
            {
                Assert.False(cmd.IsCompleted);
                Assert.Equal(i - 1, cmd.Counter);
                cmd.Execute();
                Assert.Equal(i, cmd.Counter);
            }
            
            Assert.True(cmd.IsCompleted);
        }
    }
}