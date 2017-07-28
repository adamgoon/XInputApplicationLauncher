using System;
using System.Threading.Tasks;
using System.Threading;
using SharpDX.XInput;

namespace XInputController
{
    public class MonitorController
    {
        private const int LoopDelay = 10;
        private const int Deadzone = 10000;
        private const int ScrollDivisor = 1000;
        private const int ScrollAmount = 5;
        private const UserIndex SelectedUserIndex = UserIndex.One;
        
        private Controller _controller;

        public MonitorController()
        {
            _controller = new Controller(SelectedUserIndex);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (_controller.IsConnected)
                    {
                        var gamepad = _controller.GetState().Gamepad;
                        var buttons = gamepad.Buttons;
                        var leftThumbY = gamepad.LeftThumbY;
                        var leftThumbX = gamepad.LeftThumbX;

                        CheckButtons(buttons);
                        CheckScroll(buttons, leftThumbX, leftThumbY);

                        GetBatteryInformation();
                    }

                    CheckHomeButton();

                    Thread.Sleep(LoopDelay);
                }
            });
        }

        private void GetBatteryInformation()
        {
            Events.BatteryTrigger(this, (BatteryLevel)_controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel);
        }

        private void CheckHomeButton()
        {
            if (NativeMethods.GetHomeButtonStatus((int)SelectedUserIndex))
            {
                Events.GuideTrigger(this, new EventArgs());
            }
        }

        private void CheckButtons(GamepadButtonFlags buttons)
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
        }

        private void CheckScroll(GamepadButtonFlags buttons, short leftThumbX, short leftThumbY)
        {
            if (buttons.HasFlag(GamepadButtonFlags.DPadLeft) || (leftThumbX < -Deadzone))
            {
                TriggerScrollEvent(buttons, GamepadButtonFlags.DPadLeft, leftThumbX, ScrollDirection.Left);
            }

            if (buttons.HasFlag(GamepadButtonFlags.DPadRight) || (leftThumbX > Deadzone))
            {
                TriggerScrollEvent(buttons, GamepadButtonFlags.DPadRight, leftThumbX, ScrollDirection.Right);
            }

            if (buttons.HasFlag(GamepadButtonFlags.DPadDown) || (leftThumbY < -Deadzone))
            {
                TriggerScrollEvent(buttons, GamepadButtonFlags.DPadDown, leftThumbY, ScrollDirection.Down);
            }

            if (buttons.HasFlag(GamepadButtonFlags.DPadUp) || (leftThumbY > Deadzone))
            {
                TriggerScrollEvent(buttons, GamepadButtonFlags.DPadUp, leftThumbY, ScrollDirection.Up);
            }
        }

        private void TriggerScrollEvent(GamepadButtonFlags buttons, GamepadButtonFlags button, short thumbLeft, ScrollDirection direction)
        {
            var scrollAmount = buttons.HasFlag(button) ? ScrollAmount : GetScrollAmount(thumbLeft);
            Events.ScrollTrigger(this, direction, scrollAmount);
        }

        private static int GetScrollAmount(short leftThumbX)
        {
            return (Math.Abs((int)leftThumbX) - Deadzone) / ScrollDivisor;
        }
    }
}
