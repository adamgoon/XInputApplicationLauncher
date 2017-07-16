using System;
using Utils;
using XBoxController;

namespace XBoxControlTesting
{
    class MenuWindowEventHandlers
    {
        private readonly MenuWindow _menuWindow;

        public MenuWindowEventHandlers(MenuWindow window)
        {
            _menuWindow = window;
        }

        public void StartMonitoringEvents()
        {
            Events.ScrollEventTriggered += Events_ScrollEventTriggered;
            Events.SelectEventTriggered += Events_SelectEventTriggered;
        }

        public void StopMonitoringEvents()
        {
            Events.ScrollEventTriggered -= Events_ScrollEventTriggered;
            Events.SelectEventTriggered -= Events_SelectEventTriggered;
        }

        public void ClickEvent(object sender)
        {
            Events_SelectEventTriggered(sender, new SelectEventArgs { SelectEvents = Select.A });
        }

        private void Events_SelectEventTriggered(object sender, SelectEventArgs e)
        {
            switch (e.SelectEvents)
            {
                case Select.A:
                    DispatchWindowAction(() =>
                    {
                        if (_menuWindow.listBox.SelectedIndex > -1)
                        {
                            var menuItem = (MenuItem)_menuWindow.listBox.SelectedItem;

                            Debugging.TraceInformation(string.Format($"Menu Item '{menuItem.Name}'"));
                            DispatchWindowAction(menuItem.Action);
                        }
                    });
                    break;
                case Select.B:
                    DispatchWindowAction(() =>
                    {
                         _menuWindow.Hide();
                    });
                    break;
            }
        }

        private void Events_ScrollEventTriggered(object sender, ScrollEventArgs e)
        {
            switch (e.ScrollEvent)
            {
                case ScrollDirection.Down:
                    DispatchWindowAction(() =>
                    {
                        if (_menuWindow.listBox.SelectedIndex < _menuWindow.listBox.Items.Count)
                        {
                            _menuWindow.listBox.SelectedIndex++;
                        }
                    });
                    break;
                case ScrollDirection.Up:
                    DispatchWindowAction(() =>
                    {
                        if (_menuWindow.listBox.SelectedIndex > 0)
                        {
                            _menuWindow.listBox.SelectedIndex--;
                        }
                    });
                    break;
            }
        }

        private void DispatchWindowAction(Action action)
        {
            _menuWindow.Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
