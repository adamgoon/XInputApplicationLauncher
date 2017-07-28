using System.Windows;
using XInputController;

namespace ApplicationLauncher
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MonitorController _monitorController;

        public App()
        {
            _monitorController = new MonitorController();
        }
    }
}
