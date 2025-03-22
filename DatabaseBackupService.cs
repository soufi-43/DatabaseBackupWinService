using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackupService
{
    public partial class DatabaseBackupService : ServiceBase
    {
        private string sourceFolder;
        private string destinationFolder;
        private string logFolder;


        public DatabaseBackupService()
        {
            InitializeComponent();
            CanPauseAndContinue = true; //The service supports pausing and resuming operations.

            // Enable support for OnShutdown
            CanShutdown = true; // The service is notified when the system shuts down.


            // Read log directory path from App.config
            //The service reads the log directory path from an external configuration file (App.config) for flexibility.
            logFolder = ConfigurationManager.AppSettings["LogFolder"];
            sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];


            // Validate and create directory if it doesn't exist
            if (string.IsNullOrWhiteSpace(sourceFolder))
            {
                logFolder = @"C:\sourceFolder";
                LogServiceEvent("source folder is missing in app.config we use the default " + sourceFolder);
            }

            if (string.IsNullOrWhiteSpace(destinationFolder))
            {
                destinationFolder = @"C:\destinationFolder";
                LogServiceEvent("destination folder is missing in app.config we use the default " + destinationFolder);
            }
            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = @"C:\logFolder";
                LogServiceEvent("destination folder is missing in app.config we use the default " + logFolder);
            }

            Directory.CreateDirectory(sourceFolder);
            Directory.CreateDirectory(destinationFolder);
            Directory.CreateDirectory(logFolder);
        }
        private void LogServiceEvent(string message)
        {
            string logFilePath = Path.Combine(logFolder, "FileMonitoringLog.txt");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n";
            File.AppendAllText(logFilePath, logMessage);

            if (Environment.UserInteractive)
            {
                Console.WriteLine(logMessage);
            }
        }


        public void StartConsole()
        {

        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
