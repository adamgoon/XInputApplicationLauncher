using System;
using System.Windows;
using XInputController;

namespace ApplicationLauncher
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MonitorController MonitorController { get; }

        public App()
        {
            MonitorController = new MonitorController();

            Activated += (o, e) => { MonitorController.Start(); };
            Deactivated += (o, e) => { MonitorController.Stop(); };

            MonitorController.Start();
        }
    }
}
