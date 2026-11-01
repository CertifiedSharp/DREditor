//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues.Starting
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueStarter : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="dialogues"></param>
        /// <param name="inleaving"></param>
        public abstract void StartDialogue<T>(SelectableObject actor, DialogueBase<T>[] dialogues, bool inleaving = false)
            where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="dialogue"></param>
        /// <param name="inleaving"></param>
        public abstract void StartDialogue<T>(SelectableObject actor, DialogueBase<T> dialogue, bool inleaving = false)
            where T : Line;
    }
}