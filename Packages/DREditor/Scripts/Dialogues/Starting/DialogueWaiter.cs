//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Starting
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueWaiter : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public abstract IEnumerator CheckIfInDialogueToPlay(DialogueCore dialogue);
    }
}