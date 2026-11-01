//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SetInputOnSceneEvent) + ", " + nameof(SceneEventListener) + 
        "\n\n")]
    public class InitializeFTESurfacingInput : SetInputOnSceneEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool whenDestroyedSetInput = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debugStart = false;


        protected virtual void Start()
        {
            if (debugStart)
                Event.Raise();
        }


        protected virtual void OnDestroy() => SetInputStates(whenDestroyedSetInput);
    }
}