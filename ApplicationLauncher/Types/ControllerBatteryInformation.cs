using System.ComponentModel;
using System.Runtime.CompilerServices;
using XInputController;

namespace ApplicationLauncher
{
    public class BatteryInformation : INotifyPropertyChanged
    {
        private BatteryLevel _batteryLevel = BatteryLevel.Unknown;
        private BatteryType _batteryType = BatteryType.Unknown;
        
        /// <summary>
        /// Battery level
        /// </summary>
        public BatteryLevel BatteryLevel
        {
            get { return _batteryLevel; }
            set
            {
                if (_batteryLevel == value)
                {
                    return;
                }

                _batteryLevel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Battery type
        /// </summary>
        public BatteryType BatteryType
        {
            get { return _batteryType; }
            set
            {
                if (_batteryType == value)
                {
                    return;
                }

                _batteryType = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
