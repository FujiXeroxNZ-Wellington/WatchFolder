using System;
using System.Configuration;
using FujiXerox.co.nz.FolderWatch;

namespace WatchFolder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Watcher.Run();

            //try
            //{
            //    WatchFile.MonitorFolderforFile();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            var dbConnection = ConfigurationManager.ConnectionStrings["HotFolderConnectionString"].ConnectionString;
            var logFileLocation = ConfigurationManager.AppSettings["PathtoLogFolder"].ToString();
            var hotFilesLocation = ConfigurationManager.AppSettings["PathtoHotFolder"].ToString();
            var exe = new FolderWatchConfig(logFileLocation);
            exe.GetFilesfromDirectory(hotFilesLocation, dbConnection);
        }
    }
}