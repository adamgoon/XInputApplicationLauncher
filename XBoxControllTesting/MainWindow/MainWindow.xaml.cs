using System;
using System.Collections.ObjectModel;
using System.Windows;
using XBoxController;

namespace XBoxControlTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ApplicationItem> Games { get; private set; }
        public BatteryInformation ControllerBatteryLevel { get; private set; }

        private readonly MainWindowEventHandlers _handlers;
        private readonly MonitorContoller _monitorContollerButtons;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ShowInTaskbar = false;

            _monitorContollerButtons = ((App)Application.Current).MonitorContollerButtons;

            CenterWindowOnScreen();

            Games = new ObservableCollection<ApplicationItem>(Config.LoadConfig());

            _handlers = new MainWindowEventHandlers(this);

            ControllerBatteryLevel = new BatteryInformation(_monitorContollerButtons.GetBatteryInformation());
        }

        public void ShowWindow()
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
        }
        
        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _handlers?.ClickEvent(sender);
        }

        private void CenterWindowOnScreen()
        {
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = SystemParameters.PrimaryScreenHeight - Height - 100;
        }
    }
}
