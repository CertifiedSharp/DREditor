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
    public class BoolCapsulate : IDataCapsulate
    {
        /// <summary>
        ///
        /// </summary>
        public bool value;

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

                value = HandyFields.BoolField(value, s);
            }
            else if (!string.IsNullOrEmpty(displayString))
                value = HandyFields.BoolField(value, displayString);

        }
#else
        public void Display(object o = null)
        {

        }
#endif
    }
}