//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueObjectManager : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected SelectableObject startObject = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="attemptedActor"></param>
        public virtual void AttemptSetStartingDialogueObject(SelectableObject attemptedActor)
        {
            if (attemptedActor == null)
                return;

            if (startObject != attemptedActor)
                startObject = attemptedActor;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetStartObjectNull() => startObject = null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual SelectableObject GetStartObject() => startObject;
    }
}