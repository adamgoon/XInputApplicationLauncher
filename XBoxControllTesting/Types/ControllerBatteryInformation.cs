using System.ComponentModel;
using System.Runtime.CompilerServices;
using XBoxController;

namespace XBoxControlTesting
{
    public class BatteryInformation : INotifyPropertyChanged
    {
        private BatteryLevel _level = BatteryLevel.Unknown;
        private BatteryType _type = BatteryType.Unknown;

        public BatteryInformation()
        { }

        public BatteryInformation(BatteryLevel level)
        {
            _level = level;
        }

        /// <summary>
        /// Battery level
        /// </summary>
        public BatteryLevel Level
        {
            get { return _level; }
            set
            {
                if (_level == value)
                {
                    return;
                }

                _level = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Battery type
        /// </summary>
        public BatteryType Type
        {
            get { return _type; }
            set
            {
                if (_type == value)
                {
                    return;
                }

                _type = value;
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
