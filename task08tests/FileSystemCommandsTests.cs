using System;
using System.IO;
using Xunit;
using FileSystemCommands;

namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();

            Assert.True(command.Success);
            Assert.True(command.Size > 0);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();

            Assert.True(command.Success);
            Assert.Equal(1, command.Files.Length);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_WithEmptyDirectory_ReturnsZero()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "EmptyTestDir");
            Directory.CreateDirectory(testDir);

            var command = new DirectorySizeCommand(testDir);
            command.Execute();

            Assert.True(command.Success);
            Assert.Equal(0, command.Size);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_WithNonExistentDirectory_ReturnsFalse()
        {
            var command = new DirectorySizeCommand(Path.Combine(Path.GetTempPath(), "NonExistent"));
            command.Execute();

            Assert.False(command.Success);
        }
    }
}