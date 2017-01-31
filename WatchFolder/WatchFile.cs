using RY.Utilities.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Permissions;
using Toolbox.FileManager.FileEditor.FileContentWritter;

namespace WatchFolder
{
    public static class WatchFile
    {
        private static IFileContentWritter _iFileContentWritter;

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void MonitorFolderforFile()
        {
            var argsfromCmd = System.Environment.GetCommandLineArgs();

            Console.WriteLine(argsfromCmd[1]);
            GetFilesfromDirectory(argsfromCmd[1]);
        }

        private static void GetFilesfromDirectory(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var fileInfo in dirInfo.GetFiles("*.pdf"))
            {
                if (fileInfo.Directory != null) WritetoDbTable(fileInfo.Name, fileInfo.Directory.ToString());
            }
        }

        private static void WriteToTxtFile(string pathdata)
        {
            const string outputFileLocation = @"\\13.195.108.7\Rajiv_Shared\ScanningTest\ScanTest";
            const string fileName = "WriteToTextFileContent.txt";
            _iFileContentWritter = new WriteToTextFileContent(outputFileLocation, fileName, true);
            _iFileContentWritter.WriteText(pathdata);
        }

        private static void WritetoDbTable(string documentName, string documentLocation)
        {
            //const string dbConnection = @"server=NZL0003353\SQLEXPRESS2014; Initial Catalog = FujiXerox_SolutionBuilderHotWatch;User Id=sa;Password=scuser;";
            var dbConnection = ConfigurationManager.ConnectionStrings["HotFolderConnectionString"].ConnectionString;
            try
            {
                ISqlDataWrite con = new SqlDataWrite
                {
                    ConnectionStringFromClientApp = dbConnection
                };

                const string cmd = "[dbo].[sp_cCreateHotFolderFileJob]";

                var cmdParameters = new Dictionary<string, SqlParameter>();

                var param = new SqlParameter("Docname", documentName);
                var param1 = new SqlParameter("DocLocation", documentLocation);

                cmdParameters.Add("Docname", param);
                cmdParameters.Add("DocLocation", param1);

                con.ExecuteCudCmdUsingStoredProc(cmd, cmdParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}