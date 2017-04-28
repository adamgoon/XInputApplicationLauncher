using System.ComponentModel;

namespace XBoxControlTesting
{
    public class ListBoxStuff : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private string _args;

        /// <summary>
        /// Name to be displayed
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        /// <summary>
        /// Path to the exe
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; NotifyPropertyChanged(nameof(Path)); }
        }

        /// <summary>
        /// Arguments args to be used opening program
        /// </summary>
        public string Arguments
        {
            get { return _args; }
            set { _args = value;  NotifyPropertyChanged(nameof(Arguments)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
