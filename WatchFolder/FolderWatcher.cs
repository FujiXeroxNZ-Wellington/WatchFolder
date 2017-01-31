using System;
using System.IO;
using System.Security.Permissions;

namespace WatchFolder
{
    public static class FolderWatcher
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void SystmWatcher(string path)
        {
            using (var sysWatcher = new FileSystemWatcher())
            {
                sysWatcher.Path = path;

                sysWatcher.NotifyFilter = NotifyFilters.Attributes |
     NotifyFilters.CreationTime |
     NotifyFilters.FileName |
     NotifyFilters.LastAccess |
     NotifyFilters.LastWrite |
     NotifyFilters.Size |
     NotifyFilters.Security;
                sysWatcher.Filter = "*.txt";
                sysWatcher.Changed += new FileSystemEventHandler(OnChanged);
                sysWatcher.Created += new FileSystemEventHandler(OnCreated);
                sysWatcher.EnableRaisingEvents = true;
            }
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath);
            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath);
            Console.ReadLine();
        }

        // Define the event handlers.
    }
}