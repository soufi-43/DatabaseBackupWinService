# Database Backup Windows Service

## Overview
This Windows Service automates SQL Server database backups at a specified interval. It logs all operations and can run interactively in console mode for debugging.

## Features
### 1. Automated Backups
- Performs a full backup of a specified SQL Server database.
- Saves backup files in a designated folder with a timestamp.

### 2. Dynamic Configuration
Configured via `App.config`:
- **Database Connection String**
- **Backup Folder Path**
- **Log Folder Path**
- **Backup Interval (in minutes)**

#### Example Configuration:
```xml
<appSettings>
    <add key="ConnectionString" value="Server=YOUR_SERVER;Database=YOUR_DATABASE;Integrated Security=True;" />
    <add key="BackupFolder" value="C:\DatabaseBackups" />
    <add key="LogFolder" value="C:\DatabaseBackups\Logs" />
    <add key="BackupIntervalMinutes" value="60" />
</appSettings>
```

### 3. Logging & Monitoring
- Logs service start/stop events, successful backups, and errors.
- Logs are saved in a specified log folder.

#### Sample Log Output:
```
[2024-12-16 14:00:00] Service Started.
[2024-12-16 14:10:00] Database backup successful: C:\DatabaseBackups\Backup_20241216_141000.bak
[2024-12-16 15:10:00] Error during backup: Network-related or instance-specific error occurred while establishing a connection to SQL Server.
[2024-12-16 16:00:00] Service Stopped.
```

### 4. Debugging in Console Mode
- Runs interactively in console mode for debugging.
- Displays log messages directly in the console.
- Allows manual stopping of the service.
- Uses `Environment.UserInteractive` to check if running interactively.

### 5. Deployment Requirements
- **Installer Class (`ProjectInstaller.cs`)** for deployment.
- **Service Name:** `DatabaseBackupService`.
- **Startup Type:** Automatic.
- **Service Dependencies:**
  - SQL Server (`MSSQLSERVER` or named instance)
  - Remote Procedure Call (`RpcSs`)
  - Windows Event Log (`EventLog`)

## Testing & Validation
### ✅ Successful Backup
- `.bak` files should appear in the configured folder with a timestamp.
- Log should record the success event.

### ❌ Database Connection Error
- Simulate a failure (e.g., incorrect connection string).
- Error should be logged properly.

### 🔄 Service Recovery
- Stop SQL Server, then start the backup service.
- Verify it fails due to dependency configuration.
- Restart SQL Server and confirm the service starts successfully.

### 🛠 Debugging Mode
- Run the service in console mode.
- Log messages should appear in the console.

## Additional Notes
- **Service Dependencies:** MSSQLSERVER, RpcSs, EventLog.
- **Exception Handling:** Logs detailed error messages.
- **Directory Validation:** Ensures backup/log folders exist or creates them dynamically.
- **Scalability:** Supports large databases and configurable backup intervals.

This project provides hands-on experience in developing, deploying, and managing Windows Services with real-world database operations and error handling.

---

🚀 *Feel free to contribute or report issues!*
# DatabaseBackupService
