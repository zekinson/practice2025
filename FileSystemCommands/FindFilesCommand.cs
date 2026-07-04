using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands
{
    public class FindFilesCommand : ICommand
    {
        private readonly string _directoryPath;
        private readonly string _pattern;

        public string[] Files { get; set; }
        public bool Success { get; set; }

        public FindFilesCommand(string directoryPath, string pattern)
        {
            _directoryPath = directoryPath;
            _pattern = pattern;
            Files = Array.Empty<string>();
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Success = false;
                Files = Array.Empty<string>();
                Console.WriteLine($"Каталог '{_directoryPath}' не существует");
                return;
            }

            try
            {
                Files = Directory.GetFiles(_directoryPath, _pattern, SearchOption.AllDirectories);
                Success = true;

                Console.WriteLine($"Найдено файлов по маске '{_pattern}': {Files.Length}:");
                foreach (var file in Files)
                {
                    Console.WriteLine(file);
                }
            }
            catch
            {
                Success = false;
                Files = Array.Empty<string>();
                Console.WriteLine($"Ошибка при поиске файлов в каталоге '{_directoryPath}'");
            }
        }
    }
}