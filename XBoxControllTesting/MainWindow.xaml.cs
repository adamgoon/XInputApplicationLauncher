using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using XBoxController;

namespace XBoxControlTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        public ObservableCollection<ListBoxStuff> Games { get; private set; }

        private MonitorContollerButtons _monitorContollerButtons;
        private EventHandlers _handlers;
        private NotifyIcon _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ShowInTaskbar = false;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = System.Drawing.SystemIcons.Application;
            _notifyIcon.Text = "XBoxControlTesting";
            _notifyIcon.SetVisible(false);
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            Activated += MainWindow_Activated;
            Deactivated += MainWindow_Deactivated;
            Closing += MainWindow_Closing;

            CenterWindowOnScreen();

            Games = new ObservableCollection<ListBoxStuff>(Config.LoadConfig().OrderBy(x => x.Name));

            if (Games != null) // Check for valid config
            {
                _monitorContollerButtons = new MonitorContollerButtons();
                _handlers = new EventHandlers(this);

                _monitorContollerButtons.Start();
            }
        }

        public void ShowWindow()
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
        }
        
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            _notifyIcon?.SetVisible(false);
            _monitorContollerButtons?.Start();
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            _notifyIcon?.SetVisible(true);
            _monitorContollerButtons?.Stop();
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _handlers?.ClickEvent(sender);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            _notifyIcon?.SetVisible(false);

            Dispose(true);
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = screenHeight - windowHeight - 100;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
