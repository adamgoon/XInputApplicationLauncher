using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Utils;
using XInputController;

namespace ApplicationLauncher
{
    class MainWindowEventHandlers
    {
        private const int ScrollDelay = 200;
        private const int ButtonDelay = 500;
        private readonly MainWindow _mainWindow;
        private readonly MenuWindow _menuWindow;
        private bool _mouseControlOn = false;

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
                    if (_mouseControlOn)
                    {
                        NativeMethods.LeftMouseClick();
                    }
                    else
                    {
                        DispatchWindowAction(() =>
                        {
                            if (_mainWindow.listBox.SelectedIndex > -1)
                            {
                                var applicationItem = (ApplicationItem)_mainWindow.listBox.SelectedItem;

                                Debugging.TraceInformation(string.Format($"Attempting to launch '{applicationItem.Path} {applicationItem.Arguments}'"));
                                Process.Start(applicationItem.Path, applicationItem.Arguments);
                            }
                        });
                    }

                    Thread.Sleep(ButtonDelay);
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

                    Thread.Sleep(ButtonDelay);
                    break;
                case Select.X:
                    _mouseControlOn = !_mouseControlOn;

                    Thread.Sleep(ButtonDelay);
                    break;
                case Select.B:
                    if (_mouseControlOn)
                    {
                        DispatchWindowAction(() =>
                        {
                            Debugging.TraceInformation(string.Format("Attempting to hide window"));
                            _mainWindow.Hide();
                        });
                    }

                    Thread.Sleep(ButtonDelay);
                    break;
            }
        }

        private void Events_ScrollEventTriggered(object sender, ScrollEventArgs e)
        {
            switch (e.ScrollEvent)
            {
                case ScrollDirection.Down:
                    if (_mouseControlOn)
                    {
                        var cursor = NativeMethods.GetCursorPosition();
                        NativeMethods.SetCursorPosition((int)cursor.X, (int)cursor.Y + e.ScrollAmount);
                    }
                    else
                    {
                        DispatchWindowAction(() =>
                        {
                            if (_mainWindow.listBox.SelectedIndex < _mainWindow.listBox.Items.Count)
                            {
                                _mainWindow.listBox.SelectedIndex++;
                            }
                        });

                        Thread.Sleep(ScrollDelay);
                    }
                    break;
                case ScrollDirection.Up:
                    if (_mouseControlOn)
                    {
                        var cursor = NativeMethods.GetCursorPosition();
                        NativeMethods.SetCursorPosition((int)cursor.X, (int)cursor.Y - e.ScrollAmount);
                    }
                    else
                    {
                        DispatchWindowAction(() =>
                        {
                            if (_mainWindow.listBox.SelectedIndex > 0)
                            {
                                _mainWindow.listBox.SelectedIndex--;
                            }
                        });

                        Thread.Sleep(ScrollDelay);
                    }

                    break;
                case ScrollDirection.Left:
                    if (_mouseControlOn)
                    {
                        var cursor = NativeMethods.GetCursorPosition();
                        NativeMethods.SetCursorPosition((int)cursor.X - e.ScrollAmount, (int)cursor.Y);
                    }
                    break;
                case ScrollDirection.Right:
                    if (_mouseControlOn)
                    {
                        var cursor = NativeMethods.GetCursorPosition();
                        NativeMethods.SetCursorPosition((int)cursor.X + e.ScrollAmount, (int)cursor.Y);
                    }
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
            if (!_mouseControlOn)
            {
                StopMonitoringEvents();

                DispatchWindowAction(() =>
                {
                    _menuWindow.Owner = _mainWindow;
                    _menuWindow.ShowDialog();
                    StartMonitoringEvents();
                });
            }
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
