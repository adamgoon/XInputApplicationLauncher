using System;
using System.Diagnostics;
using System.IO;
using Utils;
using XInputController;

namespace ApplicationLauncher
{
    class MainWindowEventHandlers
    {
        private readonly MainWindow _mainWindow;
        private readonly MenuWindow _menuWindow;

        public MainWindowEventHandlers(MainWindow window)
        {
            _mainWindow = window;
            _menuWindow = new MenuWindow();

            Events.GuideEventTriggered += Events_GuideEventTriggered;
            StartMonitoringEvents();
        }

        public void ClickEvent(object sender)
        {
            Events_SelectEventTriggered(sender, new SelectEventArgs { SelectEvents = Select.A });
        }

        private void StartMonitoringEvents()
        {
            Events.ScrollEventTriggered += Events_ScrollEventTriggered;
            Events.SelectEventTriggered += Events_SelectEventTriggered;
            Events.MenuEventTriggered += Events_MenuEventTriggered;
            Events.BatteryEventTriggered += Events_BatteryEventTriggered;
        }

        private void StopMonitoringEvents()
        {
            Events.ScrollEventTriggered -= Events_ScrollEventTriggered;
            Events.SelectEventTriggered -= Events_SelectEventTriggered;
            Events.MenuEventTriggered -= Events_MenuEventTriggered;
        }

        private void Events_SelectEventTriggered(object sender, SelectEventArgs e)
        {
            switch (e.SelectEvents)
            {
                case Select.A:
                    DispatchWindowAction(() =>
                    {
                        if (_mainWindow.listBox.SelectedIndex > -1)
                        {
                            var applicationItem = (ApplicationItem)_mainWindow.listBox.SelectedItem;

                            Debugging.TraceInformation(string.Format($"Attempting to launch '{applicationItem.Path} {applicationItem.Arguments}'"));
                            Process.Start(applicationItem.Path, applicationItem.Arguments);
                        }
                    });
                    break;
                case Select.Y:
                    DispatchWindowAction(() =>
                    {
                        if (_mainWindow.listBox.SelectedIndex > -1)
                        {
                            var path = ((ApplicationItem)_mainWindow.listBox.SelectedItem).Path;
                            var name = Path.GetFileNameWithoutExtension(path);
                            var processes = Process.GetProcessesByName(name);

                            if (processes.Length > 0)
                            {
                                Debugging.TraceInformation(string.Format($"Attempting to close '{path}'"));
                                foreach (var process in processes)
                                {
                                    try { process.Kill(); }
                                    catch (System.ComponentModel.Win32Exception) { /* Killing a process sometimes throws an exception */ }
                                }
                            }
                        }
                    });

                    break;
                case Select.B:
                    DispatchWindowAction(() =>
                    {
                        Debugging.TraceInformation(string.Format("Attempting to hide window"));
                        _mainWindow.Hide();
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
                        if (_mainWindow.listBox.SelectedIndex < _mainWindow.listBox.Items.Count)
                        {
                            _mainWindow.listBox.SelectedIndex++;
                        }
                    });
                    break;
                case ScrollDirection.Up:
                    DispatchWindowAction(() =>
                    {
                        if (_mainWindow.listBox.SelectedIndex > 0)
                        {
                            _mainWindow.listBox.SelectedIndex--;
                        }
                    });
                    break;
            }
        }

        private void Events_GuideEventTriggered(object sender, EventArgs e)
        {
            DispatchWindowAction(() =>
            {
                Debugging.TraceInformation("Showing App");
                _mainWindow.ShowWindow();
            });
        }

        private void Events_MenuEventTriggered(object sender, EventArgs e)
        {
            StopMonitoringEvents();

            DispatchWindowAction(() =>
            {
                _menuWindow.Owner = _mainWindow;
                _menuWindow.ShowDialog();
                StartMonitoringEvents();
            });

        }

        private void Events_BatteryEventTriggered(object sender, BatteryEventArgs e)
        {
            _mainWindow.ControllerBatteryInformation.BatteryLevel = e.BatteryLevel;
        }

        private void DispatchWindowAction(Action action)
        {
            _mainWindow.Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
