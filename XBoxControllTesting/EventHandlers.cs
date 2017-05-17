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
                    DispatchWindowAction(() =>
                    {
                        if (_window.listBox.SelectedIndex > -1)
                        {
                            var applicationItem = (ApplicationItem)_window.listBox.SelectedItem;
                            
                            Debugging.TraceInformation(string.Format("Attempting to launch '{0} {1}'", applicationItem.Path, applicationItem.Arguments));
                            var process = Process.Start(applicationItem.Path, applicationItem.Arguments);
                        }
                    });
                    break;
                case Select.Y:
                    DispatchWindowAction(() =>
                    {
                        if (_window.listBox.SelectedIndex > -1)
                        {
                            var path = ((ApplicationItem)_window.listBox.SelectedItem).Path;
                            var name = Path.GetFileNameWithoutExtension(path);
                            var processes = Process.GetProcessesByName(name);

                            if (processes.Length > 0)
                            {
                                Debugging.TraceInformation(string.Format("Attempting to close '{0}'", path));
                                foreach (var process in processes)
                                {
                                    process.Kill();
                                }
                            }
                        }
                    });

                    break;
                case Select.B:
                    DispatchWindowAction(() =>
                    {
                        Debugging.TraceInformation(string.Format("Attempting to hide window"));
                         _window.Hide();
                    });
                    break;
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
                    DispatchWindowAction(() =>
                    {
                        if (_window.listBox.SelectedIndex < _window.listBox.Items.Count)
                        {
                            _window.listBox.SelectedIndex += 1;
                        }
                    });
                    break;
                case ScrollDirection.Up:
                    DispatchWindowAction(() =>
                    {
                        if (_window.listBox.SelectedIndex > 0)
                        {
                            _window.listBox.SelectedIndex -= 1;
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
                _window.ShowWindow();

            });
        }

        private void DispatchWindowAction(Action action)
        {
            _window.Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
