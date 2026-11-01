//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISceneEventListener) +
        "\n\n")]
    public class SetStatesOnSceneEvent : MonoBehaviour, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SceneEvent> sceneEvents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<IntWithEvent> currentStates = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int newGameStateIndex = 0;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => sceneEvents.ForEach(sceneEvent => sceneEvent.RegisterListener(this));

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => sceneEvents.ForEach(sceneEvent => sceneEvent.UnregisterListener(this));

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnStartNewGameFile()
        {
            foreach (var state in currentStates)
                state.SetValue(newGameStateIndex);
        }

        /// <inheritdoc/>
        public virtual void OnEventRaised() => OnStartNewGameFile();
    }
}