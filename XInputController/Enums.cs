
namespace XInputController
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

    public enum BatteryLevel
    {
        Empty,
        Low,
        Medium,
        Full,
        Unknown
    }

    public enum BatteryType
    {
        Disconnected = 0,
        Wired = 1,
        Alkaline = 2,
        Nimh = 3,
        Unknown = 255
    }
}
