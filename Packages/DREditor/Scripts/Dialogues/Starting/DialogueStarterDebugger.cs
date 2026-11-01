//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues.Starting
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueStarterDebugger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="DebugNoDialogue"></param>
        public abstract void DebugNoCharacterDialogueCall(SelectableObject actor, DialogueCore DebugNoDialogue = null);
    }
}