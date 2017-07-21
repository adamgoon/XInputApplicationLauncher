using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace XBoxControlTesting
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public IEnumerable<MenuItem> MenuWindowItems { get; private set; }

        private readonly MenuWindowEventHandlers _handlers;

        public MenuWindow()
        {
            InitializeComponent();
            DataContext = this;

            MenuWindowItems = new ObservableCollection<MenuItem>
            {
                new MenuItem { Name = "Exit", Action= new System.Action(() => { Application.Current.Shutdown(); }) }
            };
            
            _handlers = new MenuWindowEventHandlers(this);

            Activated += (s, a) => { _handlers.StartMonitoringEvents(); };
            Deactivated += (s, a) => { _handlers.StopMonitoringEvents(); };
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _handlers?.ClickEvent(sender);
        }
    }
}
