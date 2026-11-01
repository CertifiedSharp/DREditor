//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System;
#if UNITY_EDITOR
using DREditor.Editor.Utility;
#endif

namespace DREditor.CustomEvents
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class StringCapsulate : IDataCapsulate
    {
        /// <summary>
        ///
        /// </summary>
        public string value;

        /// <summary>
        ///
        /// </summary>
        public string displayString;


#if UNITY_EDITOR
        /// <inheritdoc/>
        public virtual void Display(object o = null)
        {
            if (o is string s)
            {
                displayString = s;

                value = HandyFields.StringField(s, value);
            }
            else if (!string.IsNullOrEmpty(displayString))
                value = HandyFields.StringField(displayString, value);

        }
#else
        public void Display(object o = null)
        {

        }
#endif
    }
}