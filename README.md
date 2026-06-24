# File Monitoring Windows Service

A robust and production-ready **Windows Service** built in C# (.NET Framework 4.8) designed to monitor a specific source folder in real-time. Once a new file is detected, the service intelligently handles file-locking issues, dynamically renames the file using a unique GUID to prevent duplicates, and safely moves it to a destination folder.

The application natively supports dual-mode execution: it can run seamlessly as a standard background **Windows Service** or in **Interactive Console Mode** for easy debugging and development.

---

## 🚀 Key Features

* **Dual-Execution Mode:** Detects the environment automatically. Runs as a command-line application if `Environment.UserInteractive` is true, or boots as a native Windows Service otherwise.
* **Smart Concurrency & Retry Loop:** Unlike basic academic file watchers that crash during drag-and-drop operations of large files, this service implements an exclusive file-locking check (`FileShare.None`) with a dynamic retry loop (up to 20 attempts) to ensure the file is completely written by the OS before attempting to move it.
* **Robust Error Handling:** Wrapped entirely in global and localized `try-catch` blocks to ensure high availability—the service will never silently crash or stop unexpectedly.
* **Flexible Configuration:** Folder paths for **Source**, **Destination**, and **Logs** are read dynamically from the `App.config` file, with hardcoded desktop fallback paths if settings are missing.
* **Detailed Logging:** Chronological logging with timestamps that simultaneously outputs to a local `.txt` log file and the standard console screen.

---

## 🛠️ Configuration (`App.config`)

You can customize the watched directories directly through the application configuration file without rebuilding the project:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <appSettings>
        <add key="SourceFolder" value="C:\Users\sadeq\Desktop\File Monitoring\Source Folder" />
        <add key="DestinationFolder" value="C:\Users\sadeq\Desktop\File Monitoring\Distination Folder" />
        <add key="LogFolder" value="C:\Users\sadeq\Desktop\File Monitoring\Log Folder" />
    </appSettings>
</configuration>
