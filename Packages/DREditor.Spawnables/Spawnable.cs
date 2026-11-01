//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using UnityEngine;

namespace DREditor.Spawnables
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class Spawnable : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            InitializeDestroyCall();

            base.OnEnable();
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Initialize Destroy Call")]
        public virtual void InitializeDestroyCall()
        {
            if (dialogueEventString == "")
                SetDefaultEventString("Destroy" + gameObject.name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => Destroy(gameObject);
    }
}