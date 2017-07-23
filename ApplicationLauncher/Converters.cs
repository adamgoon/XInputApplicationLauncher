using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using XInputController;

namespace ApplicationLauncher
{
    [ValueConversion(typeof(BatteryLevel), typeof(BitmapImage))]
    class ControllerBatteryLevelConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            BatteryLevel level;

            try
            {
                level = (BatteryLevel)value;
            }
            catch
            {
                // Could not cast value to BatteryInformation type
                level = BatteryLevel.Unknown;
            }

            return GetBitmapImage(level);
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static BitmapImage GetBitmapImage(BatteryLevel level)
        {
            switch (level)
            {
                case BatteryLevel.Empty:
                    return BitmapToBitmapImage(Properties.Resources.batt_empty);
                case BatteryLevel.Low:
                    return BitmapToBitmapImage(Properties.Resources.batt_low);
                case BatteryLevel.Medium:
                    return BitmapToBitmapImage(Properties.Resources.batt_med);
                case BatteryLevel.Full:
                    return BitmapToBitmapImage(Properties.Resources.batt_full);
                case BatteryLevel.Unknown:
                default:
                    return BitmapToBitmapImage(Properties.Resources.batt_unknown);
            }
        }

        private static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
