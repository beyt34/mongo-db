using System;
using System.IO;

namespace Mongo.Console
{
    public class FileSystemOperation
    {
        public void WhenFileCreated()
        {
            // todo : FileSystemWatcher ile json tipindeki file lar sniff edilecek
            // https://docs.microsoft.com/tr-tr/dotnet/api/system.io.filesystemwatcher?view=net-5.0
            var watcher = new FileSystemWatcher(@"C:\temp\input") { NotifyFilter = NotifyFilters.FileName };

            watcher.Created += Save;
            watcher.Error += OnError;

            watcher.Filter = "*.json";
            watcher.EnableRaisingEvents = true;

            //watcher.Filter = "*.json";
        }

        private void Save(object sender, FileSystemEventArgs e)
        {
            System.Console.WriteLine($"Message: {e.Name}");
        }

        private void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());

        private void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                System.Console.WriteLine($"Message: {ex.Message}");
                System.Console.WriteLine("Stacktrace:");
                System.Console.WriteLine(ex.StackTrace);
                System.Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}