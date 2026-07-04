using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _directoryPath;

        public long Size { get; set; }
        public bool Success { get; set; }

        public DirectorySizeCommand(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Success = false;
                Console.WriteLine($"Каталог {_directoryPath} не существует");
                return;
            }

            try
            {
                Size = GetDirectorySize(_directoryPath);
                Success = true;
                Console.WriteLine($"Размер каталога '{_directoryPath}': {FormatSize(Size)}");
            }
            catch
            {
                Success = false;
                Console.WriteLine($"Ошибка при вычислении размера каталога {_directoryPath}");
            }
        }

        private long GetDirectorySize(string path)
        {
            long size = 0;

            foreach (var file in Directory.GetFiles(path))
            {
                size += new FileInfo(file).Length;
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                size += GetDirectorySize(dir);
            }

            return size;
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double bytes_ = bytes;
            int size = 0;

            while (bytes_ >= 1024 && size < sizes.Length - 1)
            {
                size++;
                bytes_ /= 1024;
            }

            return $"{bytes_:0.##} {sizes[size]}";
        }
    }
}