using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Service Running In Console Mode...");
                FileMonitoringWindowsService service = new FileMonitoringWindowsService();
                service.StartInConsole();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new FileMonitoringWindowsService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
