using System;
using System.IO;

namespace SaveFileWatcher
{

    public class DirectoryWatcher(string directoryPath, string backupDirectoryPath)
    {
        private readonly string _directoryPath = directoryPath;
        private readonly string _backupDirectoryPath = backupDirectoryPath;


        public void StartWatching()
        {
            FileSystemWatcher watcher = new(_directoryPath)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
            };

            watcher.Changed += OnFileChanged;
            watcher.Created += OnFileChanged;
            watcher.Deleted += OnFileChanged;
            watcher.Renamed += OnFileChanged;

            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Watching directory: {_directoryPath}");
            Console.WriteLine($"Backup directory: {_backupDirectoryPath}");
            Console.WriteLine("Press any key to stop watching...");
            Console.ReadKey();

            watcher.EnableRaisingEvents = false;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            string sourceFilePath = e.FullPath;
            string destinationFilePath = Path.Combine(_backupDirectoryPath, e.Name!);

            try
            {
                File.Copy(sourceFilePath, destinationFilePath, true);
                Console.WriteLine($"File backed up: {e.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to back up file: {e.Name}");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}