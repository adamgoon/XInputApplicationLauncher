using System;
using System.Windows;
using XBoxController;

namespace XBoxControlTesting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MonitorContoller MonitorContollerButtons { get; }

        public App()
        {
            MonitorContollerButtons = new MonitorContoller();

            Activated += App_Activated;
            Deactivated += App_Deactivated;

            MonitorContollerButtons.Start();
        }

        private void App_Activated(object sender, EventArgs e)
        {
            MonitorContollerButtons.Start();
        }
        private void App_Deactivated(object sender, EventArgs e)
        {
            MonitorContollerButtons.Stop();
        }
    }
}
