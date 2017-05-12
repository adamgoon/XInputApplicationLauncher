using System;

namespace XBoxController
{
    public enum ScrollDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum Select
    {
        A,
        B,
        X,
        Y
    }

    public class ScrollEventArgs : EventArgs
    {
        public ScrollDirection ScrollEvent { get; set; }
    }

    public class SelectEventArgs : EventArgs
    {
        public Select SelectEvents{ get; set; }
    }

    public static class Events
    {
        public static event EventHandler<ScrollEventArgs> ScrollEventTriggered;
        public static event EventHandler<SelectEventArgs> SelectEventTriggered;
        public static event EventHandler GuideEventTriggered;

        public static void ScrollTrigger(object sender, ScrollDirection e)
        {
            ScrollEventTriggered?.Invoke(sender, new ScrollEventArgs { ScrollEvent = e });
        }

        public static void SelectTrigger(object sender, Select e)
        {
            SelectEventTriggered?.Invoke(sender, new SelectEventArgs { SelectEvents = e });
        }

        public static void GuideTrigger(object sender, EventArgs e)
        {
            GuideEventTriggered?.Invoke(sender, e);
        }
    }
}
