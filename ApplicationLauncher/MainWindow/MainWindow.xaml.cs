using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ApplicationLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IEnumerable<ApplicationItem> Games { get; private set; }
        public BatteryInformation ControllerBatteryInformation { get; private set; }

        private readonly MainWindowEventHandlers _handlers;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ShowInTaskbar = false;

            CenterWindowOnScreen();
            
            Games = new ObservableCollection<ApplicationItem>(Config.LoadConfig());
            ControllerBatteryInformation = new BatteryInformation();
            
            _handlers = new MainWindowEventHandlers(this);          
        }
        
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _handlers?.ClickEvent(sender);
        }

        public void ShowWindow()
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
        }

        private void CenterWindowOnScreen()
        {
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = SystemParameters.PrimaryScreenHeight - Height - 100;
        }
    }
}
