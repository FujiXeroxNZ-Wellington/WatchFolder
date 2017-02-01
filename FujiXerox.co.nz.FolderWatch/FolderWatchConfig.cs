using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RY.Utilities.Data.SqlClient;
using RY.Utilities.Logger;
using RY.Utilities.Logger.FileLogger.TextFileLogger;

namespace FujiXerox.co.nz.FolderWatch
{
    public class FolderWatchConfig
    {
        private readonly IExceptionLogger _logger;
        private string _dbConnection;

        public FolderWatchConfig(string logFileLocation)
        {
            //string logFileLocation = @"D:\SolutionBuilder\HotFolderWatch\Exe\Logs";
            var logFileName = "Log-" + DateTime.Now.Date.ToString("MMM") + "-" + DateTime.Now.Year.ToString() + ".txt";
            const bool logAppendText = true;

            _logger = new LogtoTextFileWithException(logFileName, logFileLocation, logAppendText);
        }

        public void GetFilesfromDirectory(string path, string dbConnectionfromClientApp)
        {
            _dbConnection = dbConnectionfromClientApp;

            var dirInfo = new DirectoryInfo(path);
            foreach (var fileInfo in dirInfo.GetFiles("*.pdf"))
            {
                if (fileInfo.Directory != null) WritetoDbTable(fileInfo.Name, fileInfo.Directory.ToString());
            }
        }

        //private static void WritetoDbTable(string documentName, string documentLocation)
        //{
        //    //const string dbConnection = @"server=NZL0003353\SQLEXPRESS2014; Initial Catalog = FujiXerox_SolutionBuilderHotWatch;User Id=sa;Password=scuser;";
        //    var dbConnection = ConfigurationManager.ConnectionStrings["HotFolderConnectionString"].ConnectionString;
        //    try
        //    {
        //        ISqlDataWrite con = new SqlDataWrite
        //        {
        //            ConnectionStringFromClientApp = dbConnection
        //        };

        // const string cmd = "[dbo].[sp_cCreateHotFolderFileJob]";

        // var cmdParameters = new Dictionary<string, SqlParameter>();

        // var param = new SqlParameter("Docname", documentName); var param1 = new
        // SqlParameter("DocLocation", documentLocation);

        // cmdParameters.Add("Docname", param); cmdParameters.Add("DocLocation", param1);

        //        con.ExecuteCudCmdUsingStoredProc(cmd, cmdParameters);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void WritetoDbTable(string documentName, string documentLocation)
        {
            try
            {
                _logger.LogInfo("=================================================");
                _logger.LogInfo(DateTime.Now.ToString("G") + "-File watch started");
                _logger.LogInfo(DateTime.Now.ToString("G") + "-File found at: " + documentLocation);
                _logger.LogInfo(DateTime.Now.ToString("G") + "-File Name: " + documentName);
                _logger.LogInfo(DateTime.Now.ToString("G") + "-Full File Location: " + documentLocation + "\\" + documentName);
                ISqlDataWrite con = new SqlDataWrite
                {
                    ConnectionStringFromClientApp = _dbConnection
                };
                const string cmd = "[dbo].[sp_cCreateHotFolderFileJob]";

                var cmdParameters = new Dictionary<string, SqlParameter>();

                var param = new SqlParameter("Docname", documentName);
                var param1 = new SqlParameter("DocLocation", documentLocation);

                cmdParameters.Add("Docname", param);
                cmdParameters.Add("DocLocation", param1);

                con.ExecuteCudCmdUsingStoredProc(cmd, cmdParameters);

                _logger.LogInfo(DateTime.Now.ToString("G") + "-File details have been added to the database");
            }
            catch (Exception e)
            {
                _logger.LogException(e, DateTime.Now.ToString("G"));
            }
        }
    }
}