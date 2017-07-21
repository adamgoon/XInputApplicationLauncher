using System;

namespace XBoxController
{
    public class ScrollEventArgs : EventArgs
    {
        public ScrollDirection ScrollEvent { get; set; }
    }

    public class SelectEventArgs : EventArgs
    {
        public Select SelectEvents{ get; set; }
    }

    public class BatteryEventArgs : EventArgs
    {
        public BatteryLevel BatteryLevel { get; set; }
    }

    public static class Events
    {
        public static event EventHandler<ScrollEventArgs> ScrollEventTriggered;
        public static event EventHandler<SelectEventArgs> SelectEventTriggered;
        public static event EventHandler GuideEventTriggered;
        public static event EventHandler MenuEventTriggered;
        public static event EventHandler<BatteryEventArgs> BatteryEventTriggered;

        internal static void ScrollTrigger(object sender, ScrollDirection e)
        {
            ScrollEventTriggered?.Invoke(sender, new ScrollEventArgs { ScrollEvent = e });
        }

        internal static void SelectTrigger(object sender, Select e)
        {
            SelectEventTriggered?.Invoke(sender, new SelectEventArgs { SelectEvents = e });
        }

        internal static void GuideTrigger(object sender, EventArgs e)
        {
            GuideEventTriggered?.Invoke(sender, e);
        }

        internal static void MenuTrigger(object sender, EventArgs e)
        {
            MenuEventTriggered?.Invoke(sender, e);
        }

        internal static void BatteryTrigger(object sender, BatteryLevel e)
        {
            BatteryEventTriggered?.Invoke(sender, new BatteryEventArgs { BatteryLevel = e });
        }
    }
}
