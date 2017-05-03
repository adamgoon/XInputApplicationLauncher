using System.Windows.Forms;

namespace XBoxControlTesting
{
    public static class ExtensionMethods
    {
        public static void SetVisible(this NotifyIcon notifyIcon, bool visibility)
        {
            notifyIcon.Visible = visibility;
        }
    }
}
