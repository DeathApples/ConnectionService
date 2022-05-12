using System;
using System.ServiceProcess;

namespace Server
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive && args?.Length > 0)
            {
                switch (args[0])
                {
                    case "--install":
                        try
                        {
                            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                            System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { path });
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    case "--uninstall":
                        try
                        {
                            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                            System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { "/u", path });
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    default:
                        Console.WriteLine("Unknown argument!");
                        break;
                }

                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
