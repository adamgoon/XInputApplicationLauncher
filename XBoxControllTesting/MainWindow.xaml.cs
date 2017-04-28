using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using XBoxController;

namespace XBoxControlTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ListBoxStuff> Games { get; private set; }

        private MonitorContollerButtons _monitorContollerButtons;
        private EventHandlers _handlers;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Games = new ObservableCollection<ListBoxStuff>(Config.LoadConfig().OrderBy(x => x.Name));

            if (Games != null) // Check for valid config
            {
                _monitorContollerButtons = new MonitorContollerButtons();
                _handlers = new EventHandlers(this);

                _monitorContollerButtons.Start();

                Activated += MainWindow_Activated;
                Deactivated += MainWindow_Deactivated;
            }
        }

        private void MainWindow_Activated(object sender, System.EventArgs e)
        {
            _monitorContollerButtons.Start();
        }

        private void MainWindow_Deactivated(object sender, System.EventArgs e)
        {
            _monitorContollerButtons.Stop();
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _handlers.ClickEvent(sender);
        }
    }
}
