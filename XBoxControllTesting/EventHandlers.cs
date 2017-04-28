using System;
using System.Diagnostics;
using System.IO;
using Utils;
using XBoxController;

namespace XBoxControlTesting
{
    class EventHandlers
    {
        private MainWindow _window;

        public EventHandlers(MainWindow window)
        {
            _window = window;
            Events.GuideEventTriggered += Events_GuideEventTriggered;
            Events.ScrollEventTriggered += Events_ScrollEventTriggered;
            Events.SelectEventTriggered += Events_SelectEventTriggered;
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
                    DispatchAction((() =>
                    {
                        if (_window.listBox.SelectedIndex > -1)
                        {
                            var path = ((ListBoxStuff)_window.listBox.SelectedItem).Path;
                            var args = ((ListBoxStuff)_window.listBox.SelectedItem).Arguments;
                            
                            Debugging.TraceInformation(string.Format("Attempting to launch '{0} {1}'", path, args));
                            var process = Process.Start(path, args);
                        }
                    }));
                    break;
                case Select.Y:
                    DispatchAction((() =>
                    {
                        if (_window.listBox.SelectedIndex > -1)
                        {
                            var path = ((ListBoxStuff)_window.listBox.SelectedItem).Path;
                            var name = Path.GetFileNameWithoutExtension(path);
                            
                            Process[] processes = Process.GetProcessesByName(name);
                            if (processes.Length == 0)
                            {
                                Debugging.TraceInformation(string.Format("Cannot close, could not find '{0}'", path));
                            }
                            else
                            {
                                Debugging.TraceInformation(string.Format("Attempting to close '{0}'", path));
                                foreach (var process in processes)
                                {
                                    process.Kill();
                                }
                            }
                        }
                    }));

                    break;
                case Select.B:
                case Select.X:
                default:
                    /* Do Something */
                    break;
            }
        }

        private void Events_ScrollEventTriggered(object sender, ScrollEventArgs e)
        {
            switch (e.ScrollEvent)
            {
                case ScrollDirection.Down:
                    DispatchAction((() =>
                    {
                        if (_window.listBox.SelectedIndex < _window.listBox.Items.Count)
                        {
                            _window.listBox.SelectedIndex += 1;
                        }
                    }));
                    break;
                case ScrollDirection.Up:
                    DispatchAction((() =>
                    {
                        if (_window.listBox.SelectedIndex > 0)
                        {
                            _window.listBox.SelectedIndex -= 1;
                        }
                    }));
                    break;
            }
        }

        private void Events_GuideEventTriggered(object sender, EventArgs e)
        {
            DispatchAction((() =>
            {
                Debugging.TraceInformation("Showing App");
                _window.WindowState = System.Windows.WindowState.Normal;
                _window.Activate();
            }));
        }

        private void DispatchAction(Action action)
        {
            _window.Dispatcher.BeginInvoke((Action)(() =>
            {
                action();
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
