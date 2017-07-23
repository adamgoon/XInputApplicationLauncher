using System;
using System.Globalization;

namespace Utils
{
    public static class Debugging
    {
        public static void TraceInformation(string message)
        {
#if DEBUG
            System.Diagnostics.Trace.TraceInformation($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}: {message}");
#endif
        }
    }
}
