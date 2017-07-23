using System.Runtime.InteropServices;

namespace XInputController
{
    public static class NativeMethods
    {
        private static XINPUT_GAMEPAD_SECRET xgs;

        [DllImport("xinput1_4.dll", EntryPoint = "#100")]
        private static extern int secret_get_gamepad(int playerIndex, out XINPUT_GAMEPAD_SECRET struc);

        public struct XINPUT_GAMEPAD_SECRET
        {
            public uint eventCount;
            public ushort wButtons;
            public byte bLeftTrigger;
            public byte bRightTrigger;
            public short sThumbLX;
            public short sThumbLY;
            public short sThumbRX;
            public short sThumbRY;
        }

        static public bool GetHomeButtonStatus(int playerIndex)
        {
            secret_get_gamepad(playerIndex, out xgs);

            return ((xgs.wButtons & 0x0400) != 0);
        }
    }
}
