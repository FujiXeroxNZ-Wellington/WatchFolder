using System;

namespace WatchFolder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //  Watcher.Run();

            try
            {
                WatchFile.MonitorFolderforFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}