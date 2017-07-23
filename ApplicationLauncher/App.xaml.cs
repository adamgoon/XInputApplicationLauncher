using System.Windows;
using XInputController;

namespace ApplicationLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MonitorContoller MonitorContoller { get; }

        public App()
        {
            MonitorContoller = new MonitorContoller();

            Activated += (o, e) => { MonitorContoller.Start(); };
            Deactivated += (o, e) => { MonitorContoller.Stop(); };

            MonitorContoller.Start();
        }
    }
}
