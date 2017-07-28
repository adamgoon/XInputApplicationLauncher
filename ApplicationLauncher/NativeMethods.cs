using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace ApplicationLauncher
{
    public static class NativeMethods
    {
        [DllImport("User32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("User32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        private static extern void MouseEvent(UInt32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, IntPtr dwExtraInf);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        public static Point GetCursorPosition()
        {
            var pt = new Win32Point(); ;
            GetCursorPos(ref pt);

            return new Point
            {
                X = pt.X,
                Y = pt.Y
            };
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void LeftMouseClick()
        {
            var pos = GetCursorPosition();
            MouseEvent(MOUSEEVENTF_LEFTDOWN, (int)pos.X, (int)pos.Y, 0, new IntPtr(0));
            MouseEvent(MOUSEEVENTF_LEFTUP, (int)pos.X, (int)pos.Y, 0, new IntPtr(0));
        }
    }
}
