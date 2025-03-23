using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;


namespace DatabaseBackupService
{
    public partial class DatabaseBackupService : ServiceBase
    {
        private Timer backupTimer; 
        private string ConnectionString;
        private string BackupFolder;
        private string logFolder;
        private int backupIntervalMinutes;


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
            backupIntervalMinutes = int.TryParse( ConfigurationManager.AppSettings["BackupIntervalMinutes"],out int interval ) ? interval : 60 ;


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
            OnStart(null);
            Console.WriteLine("press any key to stop the service");
            Console.ReadKey();
            OnStop();


        }

        protected override void OnStart(string[] args)
        {
            LogServiceEvent("Service Started");

            backupTimer = new Timer(

                callback: PerformBackup,
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromMinutes(backupIntervalMinutes) 
                );

            LogServiceEvent($"backup initiated every {backupIntervalMinutes} minutes ");


           
        }

        private void PerformBackup(object state)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFile = Path.Combine(BackupFolder, $"Backup_{timestamp} horse clinic .bak");

                string sqlBackup = $@"
            BACKUP DATABASE [HorseClinic]
            TO DISK = '{backupFile}'
            WITH FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10;";

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlBackup, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                LogServiceEvent($"Backup successful: {backupFile}");
            }
            catch (Exception ex)
            {
                LogServiceEvent($"Backup failed: {ex.Message}");
            }
        }

        

        protected override void OnStop()
        {
            backupTimer?.Dispose(); 
            LogServiceEvent("Service Stopped");

        }
    }
}
