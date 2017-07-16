using System;
using SharpDX.XInput;

namespace XBoxController
{
    public class MonitorContoller
    {
        private const int LoopDelay = 10;
        private const int ScrollDelay = 200;
        private const int ButtonDelay = 500;
        private const int NoDelay = 0;
        private const int Deadzone = 20000;

        private bool _stop = false;
        private UserIndex _userIndex = UserIndex.One;
        private Controller _controller;

        public void Start()
        {
            _stop = false;
        }

        public void Stop()
        {
            _stop = true;
        }

        public MonitorContoller()
        {
            _controller = new Controller(_userIndex);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var delay = LoopDelay;

                    if (!_stop)
                    {
                        delay = CheckInputs();
                    }

                    delay = Math.Max(delay, CheckHomeButton());

                    System.Threading.Thread.Sleep(delay);
                }
            });
        }

        public BatteryLevel GetBatteryInformation()
        {
            if (_controller.IsConnected)
            {
                return (BatteryLevel)_controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel;
            }

            return BatteryLevel.Unknown;
        }

        private int CheckHomeButton()
        {
            if (NativeMethods.GetHomeButtonStatus((int)_userIndex))
            {
                Events.GuideTrigger(this, new EventArgs());
                return ScrollDelay;
            }

            return NoDelay;
        }

        private int CheckInputs()
        {
            int result = NoDelay;

            if (_controller.IsConnected)
            {
                var gamepad = _controller.GetState().Gamepad;

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                {
                    Events.SelectTrigger(this, Select.A);
                    result = ButtonDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                {
                    Events.SelectTrigger(this, Select.B);
                    result = ButtonDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.X))
                {
                    Events.SelectTrigger(this, Select.X);
                    result = ButtonDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                {
                    Events.SelectTrigger(this, Select.Y);
                    result = ButtonDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Start))
                {
                    Events.MenuTrigger(this, new EventArgs());
                    result = ButtonDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown) || (gamepad.LeftThumbY < -Deadzone))
                {
                    Events.ScrollTrigger(this, ScrollDirection.Down);
                    result = ScrollDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp) || (gamepad.LeftThumbY > Deadzone))
                {
                    Events.ScrollTrigger(this, ScrollDirection.Up);
                    result = ScrollDelay;
                }
            }

            return result;
        }
    }
}
