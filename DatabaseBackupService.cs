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
        private string ConnectionString;
        private string BackupFolder;
        private string logFolder;


        public DatabaseBackupService()
        {
            InitializeComponent();
            CanPauseAndContinue = true; //The service supports pausing and resuming operations.

            // Enable support for OnShutdown
            CanShutdown = true; // The service is notified when the system shuts down.


            // Read log directory path from App.config
            //The service reads the log directory path from an external configuration file (App.config) for flexibility.
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            logFolder = ConfigurationManager.AppSettings["LogFolder"];
            BackupFolder = ConfigurationManager.AppSettings["BackupFolder"];


            // Validate and create directory if it doesn't exist
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = @"Server=.;Database=HorseClinic;User Id=sa;Password=sa123456;Integrated Security=True;";
                LogServiceEvent("connection string is missing in app.config we use the default " + ConnectionString);
            }

            if (string.IsNullOrWhiteSpace(BackupFolder))
            {
                BackupFolder = @"C:\BackupFolder";
                LogServiceEvent("BackupFolder folder is missing in app.config we use the default " + BackupFolder);
            }
            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = @"C:\logFolder";
                LogServiceEvent("destination folder is missing in app.config we use the default " + logFolder);
            }

            Directory.CreateDirectory(BackupFolder);
            Directory.CreateDirectory(logFolder);
        }
        private void LogServiceEvent(string message)
        {
            string logFilePath = Path.Combine(logFolder, "DatabaseBackupWinLog.txt");
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
