using System;
using System.Threading.Tasks;
using System.Threading;
using SharpDX.XInput;
using System.Collections.Generic;
using System.Linq;

namespace XInputController
{
    public class MonitorController
    {
        private const int LoopDelay = 10;
        private const int ScrollDelay = 200;
        private const int ButtonDelay = 500;
        private const int Deadzone = 20000;
        private const UserIndex SelectedUserIndex = UserIndex.One;
        private const GamepadButtonFlags ButtonMask = GamepadButtonFlags.A | GamepadButtonFlags.B | GamepadButtonFlags.X | GamepadButtonFlags.Y | GamepadButtonFlags.Start;

        private bool _stop = false;
        private Controller _controller;

        public void Start()
        {
            _stop = false;
        }

        public void Stop()
        {
            _stop = true;
        }

        public MonitorController()
        {
            _controller = new Controller(SelectedUserIndex);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var delays = new List<int>();

                    if (_controller.IsConnected)
                    {
                        if (!_stop)
                        {
                            var gamepad = _controller.GetState().Gamepad;
                            var buttons = gamepad.Buttons;
                            var leftThumbY = gamepad.LeftThumbY;

                            delays.Add(CheckButtons(buttons));
                            delays.Add(CheckScroll(buttons, leftThumbY));
                        }

                        GetBatteryInformation();
                    }

                    delays.Add(CheckHomeButton());

                    Thread.Sleep(delays.Max());
                }
            });
        }

        private void GetBatteryInformation()
        {
            Events.BatteryTrigger(this, (BatteryLevel)_controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel);
        }

        private int CheckHomeButton()
        {
            if (NativeMethods.GetHomeButtonStatus((int)SelectedUserIndex))
            {
                Events.GuideTrigger(this, new EventArgs());

                return ScrollDelay;
            }

            return LoopDelay;
        }

        private int CheckButtons(GamepadButtonFlags buttons)
        {
            if (buttons.HasFlag(GamepadButtonFlags.A))
            {
                Events.SelectTrigger(this, Select.A);
            }

            if (buttons.HasFlag(GamepadButtonFlags.B))
            {
                Events.SelectTrigger(this, Select.B);
            }

            if (buttons.HasFlag(GamepadButtonFlags.X))
            {
                Events.SelectTrigger(this, Select.X);
            }

            if (buttons.HasFlag(GamepadButtonFlags.Y))
            {
                Events.SelectTrigger(this, Select.Y);
            }

            if (buttons.HasFlag(GamepadButtonFlags.Start))
            {
                Events.MenuTrigger(this, new EventArgs());
            }

            return ((buttons & ButtonMask) > 0) ? ButtonDelay : LoopDelay;
        }
        
        private int CheckScroll(GamepadButtonFlags buttons, short leftThumbY)
        {
            int delay = LoopDelay;

            if (buttons.HasFlag(GamepadButtonFlags.DPadDown) || (leftThumbY < -Deadzone))
            {
                Events.ScrollTrigger(this, ScrollDirection.Down);
                delay = ScrollDelay;
            }

            if (buttons.HasFlag(GamepadButtonFlags.DPadUp) || (leftThumbY > Deadzone))
            {
                Events.ScrollTrigger(this, ScrollDirection.Up);
                delay = ScrollDelay;
            }

            return delay;
        }
    }
}
