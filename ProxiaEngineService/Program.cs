using System;
using System.ServiceProcess;

namespace ProxiaEngineService
{
    static class Program
    {
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new ProxiaService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {

                string[] args = Properties.Settings.Default.ConsoleParameters.Split();
                App.Run(args);
            }
        }
    }
}
