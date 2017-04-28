using System;
using SharpDX.XInput;
using Utils;

namespace XBoxController
{
    public class MonitorContollerButtons
    {
        private bool _stop = false;
        const int LoopDelay = 10;
        const int ScrollDelay = 200;
        const int ButtonDelay = 500;

        public MonitorContollerButtons()
        {
            Controller controller = new Controller(UserIndex.One);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    int delay = LoopDelay;

                    if (!_stop)
                    {
                        delay = CheckInputs(controller);
                    }

                    delay = Math.Max(delay, CheckHomeButton());

                    System.Threading.Thread.Sleep(delay);
                }
            });
        }

        public void Start()
        {
            _stop = false;
        }

        public void Stop()
        {
            _stop = true;
        }

        private int CheckHomeButton()
        {
            if (NativeMethods.GetHomeButtonStatus((int)UserIndex.One))
            {
                Debugging.TraceInformation(string.Format("Guide Button Pressed"));
                Events.GuideTrigger(this, new EventArgs());
                return ScrollDelay;
            }

            return 0;
        }

        private int CheckInputs(Controller controller)
        {
            int result = 0;

            if (controller.IsConnected)
            {
                var gamepad = controller.GetState().Gamepad;

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

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown) || (gamepad.LeftThumbY < -20000))
                {
                    Debugging.TraceInformation(string.Format("Down requested"));
                    Events.ScrollTrigger(this, ScrollDirection.Down);
                    result = ScrollDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp) || (gamepad.LeftThumbY > 20000))
                {
                    Debugging.TraceInformation(string.Format("Up Requested"));
                    Events.ScrollTrigger(this, ScrollDirection.Up);
                    result = ScrollDelay;
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft) || (gamepad.LeftThumbX < -20000))
                {
                    Events.ScrollTrigger(this, ScrollDirection.Left);
                }

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight) || (gamepad.LeftThumbX > 20000))
                {
                    Events.ScrollTrigger(this, ScrollDirection.Right);
                }
            }

            return result;
        }
    }
}
