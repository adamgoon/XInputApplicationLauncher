using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using XBoxController;

namespace XBoxControlTesting
{
    class ControllerBatteryLevelConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            BatteryLevel level;

            try
            {
                level = ((BatteryInformation)value).Level;
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
            Bitmap bitmap;

            switch (level)
            {
                case BatteryLevel.Empty:
                    bitmap = Properties.Resources.batt_empty;
                    break;
                case BatteryLevel.Low:
                    bitmap = Properties.Resources.batt_low;
                    break;
                case BatteryLevel.Medium:
                    bitmap = Properties.Resources.batt_med;
                    break;
                case BatteryLevel.Full:
                    bitmap = Properties.Resources.batt_full;
                    break;
                case BatteryLevel.Unknown:
                default:
                    bitmap = Properties.Resources.batt_unknown;
                    break;
            }

            var bitmapImage = new BitmapImage();

            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
