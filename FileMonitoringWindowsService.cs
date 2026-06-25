using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;

namespace File_Monitoring_Windows_Service
{
    public partial class FileMonitoringWindowsService : ServiceBase
    {
        private static FileSystemWatcher fileWatcher;
        private string sourceFolder;
        private string destinationFolder;
        private string logFolder;
        public FileMonitoringWindowsService()
        {
            InitializeComponent();

            sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
            logFolder = ConfigurationManager.AppSettings["LogFolder"];

            if(string.IsNullOrWhiteSpace(sourceFolder))
            {
                sourceFolder = @"C:\Users\sadeq\Desktop\File Monitoring\Source Folder";
            }
            if (string.IsNullOrWhiteSpace(destinationFolder))
            {
                destinationFolder = @"C:\Users\sadeq\Desktop\File Monitoring\Distination Folder";
            }
            if (string.IsNullOrWhiteSpace(logFolder))
            {
                logFolder = @"C:\Users\sadeq\Desktop\File Monitoring\Log Folder";
            }

            Directory.CreateDirectory(sourceFolder);
            Directory.CreateDirectory(destinationFolder);
            Directory.CreateDirectory(logFolder);
        }
        protected override void OnStart(string[] args)
        {
            Log("Service Started");
            fileWatcher = new FileSystemWatcher
            {
                Path = sourceFolder,
                Filter = "*.*",
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };
            fileWatcher.Created += OnFileCreated;
            Log("File monitoring started on folder: " + sourceFolder);
        }
        protected override void OnStop()
        {
            fileWatcher.EnableRaisingEvents = false;
            fileWatcher.Dispose();
            Log("Service Stopped");
        }
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                Log("New File Detected");
                int Attempts = 0;
                int maxAttempts = 20;
                bool IsFileReady = false;
                while(Attempts<maxAttempts)
                {
                    try
                    {
                        using(FileStream stream = File.Open(e.FullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            IsFileReady = true;
                            break;
                        }
                    }
                    catch(IOException)
                    {
                        Attempts++;
                        System.Threading.Thread.Sleep(500);
                    }
                }
                if (IsFileReady && File.Exists(e.FullPath))
                {
                    string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(e.Name)}";
                    string destinationFile = Path.Combine(destinationFolder, newFileName);
                    File.Move(e.FullPath, destinationFile);
                    Log($"File Moved: {e.FullPath} -> {destinationFile}");
                }
                else
                {
                    Log($"Could not process file: {e.FullPath}. File remained locked or missing after timeout.");
                }
            }
            catch(Exception ex)
            {
                Log($"Error Processing File: {e.FullPath}. Exception: {ex.Message}");
            }
        }
        private void Log(string message)
        {
            string logFilePath = Path.Combine(logFolder, "FileMonitoringWindowsService.txt");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}, {message}]";
            
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            
            if(Environment.UserInteractive)
            {
                Console.WriteLine(logMessage);
            }
        }
        public void StartInConsole()
        {
            OnStart(null);
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine();

            OnStop();

        }
    }
}
