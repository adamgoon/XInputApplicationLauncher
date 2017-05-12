using System;
using SharpDX.XInput;

namespace XBoxController
{
    public class MonitorContollerButtons
    {
        private const int LoopDelay = 10;
        private const int ScrollDelay = 200;
        private const int ButtonDelay = 500;
        private const int NoDelay = 0;
        private const int Deadzone = 20000;

        private bool _stop = false;
        
        public void Start()
        {
            _stop = false;
        }

        public void Stop()
        {
            _stop = true;
        }

        public MonitorContollerButtons()
        {
            var controller = new Controller(UserIndex.One);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var delay = LoopDelay;

                    if (!_stop)
                    {
                        delay = CheckInputs(controller);
                    }

                    delay = Math.Max(delay, CheckHomeButton());

                    System.Threading.Thread.Sleep(delay);
                }
            });
        }

        private int CheckHomeButton()
        {
            if (NativeMethods.GetHomeButtonStatus((int)UserIndex.One))
            {
                Events.GuideTrigger(this, new EventArgs());
                return ScrollDelay;
            }

            return NoDelay;
        }

        private int CheckInputs(Controller controller)
        {
            int result = NoDelay;

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
