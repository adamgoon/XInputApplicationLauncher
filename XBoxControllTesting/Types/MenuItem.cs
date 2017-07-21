using System;

namespace XBoxControlTesting
{
    public class MenuItem
    {
        /// <summary>
        /// Name to be displayed
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Action to be performed when menu item is activated
        /// </summary>
        public Action Action { get; set; }
    }
}
