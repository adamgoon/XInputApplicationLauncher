using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ApplicationLauncher
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public int MenuWindowItemHeight { get { return 35; } }
        public IEnumerable<MenuItem> MenuWindowItems { get; private set; }
        public int CustomHeight
        {
            get
            {
                return WindowPadding + (MenuWindowItems.Count() * MenuWindowItemHeight);
            }
            set { }
        }

        private const int WindowPadding = 10;

        private readonly MenuWindowEventHandlers _handlers;

        public MenuWindow()
        {
            InitializeComponent();
            DataContext = this;
            ShowInTaskbar = false;

            MenuWindowItems = new ObservableCollection<MenuItem>
            {
                new MenuItem { Name = Properties.Resources.Menu_Hide, Action= new Action(() => { Hide();  Application.Current.MainWindow.Hide(); }) },
                new MenuItem { Name = Properties.Resources.Menu_Close, Action= new Action(() => { Hide(); }) },
                new MenuItem { Name = Properties.Resources.Menu_Exit, Action= new Action(() => { Application.Current.Shutdown(); }) }
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
