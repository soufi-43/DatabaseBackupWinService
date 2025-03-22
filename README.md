# Database Backup Service

## Overview
The **Database Backup Service** is a Windows Service that automates SQL Server database backups, ensuring data reliability and security. This project demonstrates expertise in .NET development, Windows Services, and SQL Server management, with a focus on automation, error handling, and system resilience.

## Features

### ✅ Automated Database Backups
- Performs full backups of a specified SQL Server database.
- Saves backup files with timestamped filenames for easy tracking.
- Configurable backup intervals (default: every 60 minutes).

### ⚙️ Dynamic Configuration
- Uses **App.config** to allow seamless configuration of:
  - **Database Connection String** – Define the SQL Server instance and database.
  - **Backup Folder Path** – Specify where backups are stored.
  - **Log Folder Path** – Keep detailed logs of operations.
  - **Backup Interval** – Set the frequency of backups in minutes.

Example **App.config** settings:
```xml
<appSettings>
    <add key="ConnectionString" value="Server=YOUR_SERVER;Database=YOUR_DATABASE;Integrated Security=True;" />
    <add key="BackupFolder" value="C:\DatabaseBackups" />
    <add key="LogFolder" value="C:\DatabaseBackups\Logs" />
    <add key="BackupIntervalMinutes" value="60" />
</appSettings>
```

### 📜 Logging & Monitoring
- Creates detailed logs of service operations, including:
  - **Service start/stop events**
  - **Successful backups** (including file paths)
  - **Errors during backup or connection issues**

**Sample Log Output:**
```
[2024-12-16 14:00:00] Service Started.
[2024-12-16 14:10:00] Database backup successful: C:\DatabaseBackups\Backup_20241216_141000.bak
[2024-12-16 15:10:00] Error: Network-related issue while connecting to SQL Server.
[2024-12-16 16:00:00] Service Stopped.
```

### 🛠 Debugging Mode
- Runs interactively in **console mode** for easy debugging.
- Displays log messages directly in the console.
- Allows manual service termination when needed.

### 🚀 Deployment & Reliability
- Includes an **Installer Class (ProjectInstaller.cs)** for seamless deployment.
- Configured to run as a **Windows Service** named `DatabaseBackupService`.
- Starts automatically on system boot.
- **Service Dependencies:** Ensures SQL Server (`MSSQLSERVER`), Remote Procedure Call (`RpcSs`), and Windows Event Log (`EventLog`) are available before execution.

## Testing & Validation
This project follows a robust testing approach to ensure reliability:

### ✅ Functional Tests
- The service automatically creates backup files in the specified folder with correct timestamps.
- Log entries are generated for each successful backup operation.

### ❌ Error Handling Tests
- Database connection failures are simulated by providing incorrect credentials.
- Detailed error logs are generated, and the service gracefully handles failures.

### 🔄 Service Recovery Tests
- The SQL Server service is stopped, and the backup service attempts to start.
- The service does not start due to missing dependencies.
- SQL Server is restarted, and the backup service resumes operation automatically.

### 🛠 Debugging Tests
- The service runs in **console mode**, displaying log messages in real-time for easier troubleshooting.

## Why This Project Stands Out
- **Industry-Ready:** Demonstrates expertise in Windows Services, SQL Server, and automation.
- **Scalability:** Supports large databases with configurable backup intervals.
- **Security & Reliability:** Handles failures gracefully and ensures seamless recovery.
- **Real-World Application:** Provides practical exposure to service deployment and maintenance.

## Getting Started
1. Clone the repository:
   ```sh
   git clone https://github.com/YOUR_GITHUB_USERNAME/DatabaseBackupService.git
   ```
2. Update **App.config** with your SQL Server settings.
3. Build and install the Windows Service.
4. Monitor logs for backup and error events.

## Contributions
This project is open for improvements! Feel free to submit **issues** and **pull requests** to enhance functionality.

---
💡 **Looking for a skilled .NET developer?** Let’s connect! 🚀
