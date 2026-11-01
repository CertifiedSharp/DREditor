//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    public interface IDebateGroupManager
    {
        /// <summary>
        ///
        /// </summary>
        UnityEvent OnPanelsResolved { get; }

        /// <summary>
        ///
        /// </summary>
        UnityEvent<DialogueCore> OnStopPanels { get; }

        /// <summary>
        ///
        /// </summary>
        UnityEvent OnRestartPanels { get; }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="debate"></param>
        IEnumerator PlayGroups<T>(DialogueBase<T> debate);
    }
}