using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processinstaller;
        private ServiceInstaller serviceinstaller;
        public Installer1()
        {
            InitializeComponent();
            processinstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            serviceinstaller = new ServiceInstaller
            {
                ServiceName = "FileMonitoringService",
                DisplayName = "File Monitoring Service",
                StartType = ServiceStartMode.Automatic,
                Description = "This is my file monitoring service.",
                ServicesDependedOn = new string[] { "RpcSs", "EventLog", "LanmanWorkstation" }
            };
            Installers.Add(serviceinstaller);
            Installers.Add(processinstaller);
        }
    }
}
