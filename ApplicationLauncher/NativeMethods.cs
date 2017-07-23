using System;
using System.Runtime.InteropServices;

namespace ApplicationLauncher
{
    public static class NativeMethods
    {
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const UInt32 WS_DISABLED = 0x8000000;
        public const UInt32 WS_VISIBLE = 0x10000000;
        public const Int32 GWL_STYLE = -16;

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindowNative(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        static extern IntPtr GetForegroundWindowNative();

        public static void ShowWindow(IntPtr hWnd, int nCmdShow)
        {
            ShowWindowNative(hWnd, nCmdShow);
        }

        public static IntPtr GetForegroundWindow()
        {            
            return GetForegroundWindowNative();
        }
    }
}
