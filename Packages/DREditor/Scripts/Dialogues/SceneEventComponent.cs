//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public class SceneEventComponent : DialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent sceneEvent = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeSceneEvent() => sceneEvent.Raise();
    }
}