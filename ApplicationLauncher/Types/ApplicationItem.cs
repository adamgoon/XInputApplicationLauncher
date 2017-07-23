using System.Windows.Media;

namespace ApplicationLauncher
{
    public class ApplicationItem
    {
        /// <summary>
        /// Name to be displayed
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to the exe
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Arguments args to be used opening program
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Icon to be disaplyed
        /// </summary>
        public ImageSource Icon { get; set; }
    }
}
