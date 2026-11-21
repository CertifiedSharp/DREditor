//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor
{
    /// <summary>
    /// Attribute to be put on a manager to handle a custom dialogue event so it shows up in the Custom Dialogue Event GUI. <br/>
    /// MAKE SURE THE STRING IS THE SAME AS THE ONE USED IN LISTENING DialogueEventSystem.StartListening.
    /// </summary>
    /// <remarks>
    /// This will break if the following occurs: <br/>
    /// - A Class with this Attribute is renamed <br/>
    /// - The Attribute tag on the Class is Shuffled around (if more than one attribute was givin) <br/> <br/>
    /// If this occurs please go to => DREditor/Utility/Verify/Custom Events <br/>
    /// </remarks>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CustomDialogueEventAttribute : Attribute
    {
        /// <summary>
        /// The string name for the event to be called through the dialogue system.
        /// </summary>
        public string eventName;

        /// <summary>
        /// Specified data class for the event to be sent during invocation. It is suggested the class implement IDataCapsulate.
        /// </summary>
        public Type type;

        /// <summary>
        /// Miscellaneous data to pass on to the GUI process for this custom dialogue event.
        /// </summary>
        public object pass = null;


        /// <summary>
        /// Attribute to be put on a manager to handle a custom dialogue event so it shows up in the Custom Dialogue Event GUI. <br/>
        /// MAKE SURE THE STRING IS THE SAME AS THE ONE USED IN LISTENING DialogueEventSystem.StartListening.
        /// </summary>
        /// <param name="eventName">The string name for the event to be called through the dialogue system.</param>
        /// <param name="type">Specified data class for the event to be sent during invocation. 
        /// It is suggested the class implement IDataCapsulate.</param>
        /// <param name="pass">Miscellaneous data to pass on to the GUI process for this custom dialogue event.</param>
        /// <remarks>
        /// This will break if the following occurs: <br/>
        /// - A Class with this Attribute is renamed <br/>
        /// - The Attribute tag on the Class is Shuffled around (if more than one attribute was givin) <br/> <br/>
        /// If this occurs please go to => DREditor/Utility/Verify/Custom Events <br/>
        /// </remarks>
        public CustomDialogueEventAttribute(string eventName, Type type = null, object pass = null)
        {
            this.eventName = eventName;

            this.type = type;

            this.pass = pass;
        }
    }
}