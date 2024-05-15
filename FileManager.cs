using System;
using System.IO;

namespace SaveFileWatcher
{
    public class FileWatcher
    {
        private string filePath;
        private FileSystemWatcher fileSystemWatcher;

        public FileWatcher(string filePath)
        {
            this.filePath = filePath;
            fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
            fileSystemWatcher.Changed  += OnFileChanged;
            fileSystemWatcher.Created += OnFileChanged;

        }

        public void StartWatching()
        {
            fileSystemWatcher.EnableRaisingEvents = true;
            Console.WriteLine($"Watching file: {filePath}");
            Console.WriteLine("Press any key to stop watching...");
            Console.ReadKey();
            StopWatching();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File changed: {e.FullPath}");
        }

        private void StopWatching()
        {
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
            Console.WriteLine("Stopped watching file.");
        }
    }

    
}