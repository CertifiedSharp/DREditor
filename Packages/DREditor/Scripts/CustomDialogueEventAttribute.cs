//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    /// 
    /// <remarks>
    ///
    /// </remarks>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CustomDialogueEventAttribute : Attribute
    {
        public string eventName;
        public Type type;
        public object pass = null;

        public CustomDialogueEventAttribute(string eventName, Type type = null, object capsulateData = null)
        {
            this.eventName = eventName;

            this.type = type;

            pass = capsulateData;
        }
    }
}