//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SerializedItem
    {
        /// <summary>
        ///
        /// </summary>
        public string type;

        /// <summary>
        ///
        /// </summary>
        public string value;

        public SerializedItem(string type, string value)
        {
            this.type = type;

            this.value = value;
        }
    }
}