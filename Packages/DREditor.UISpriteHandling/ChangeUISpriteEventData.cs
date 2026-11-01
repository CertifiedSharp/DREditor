//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System;
using UnityEngine;


#if UNITY_EDITOR
using DREditor.Editor.Utility;
#endif

namespace DREditor.UISpriteHandling
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ChangeUISpriteEventData : IDataCapsulate
    {
        /// <summary>
        ///
        /// </summary>
        public CharacterUISpriteData characterUIData = null;


    #if UNITY_EDITOR
        /// <inheritdoc/>
        public void Display(object o)
        {
            HandyFields.Label("The data for telling the protag manager how to swap the protag.");

            characterUIData = HandyFields.UnityField(new GUIContent("CharacterUIData:",
                "Asset containing info related to how a character sprite is displayed to the UI."),
                characterUIData);
        }
    #else
        public void Display(object o = null)
        {

        }
    #endif
    }
}