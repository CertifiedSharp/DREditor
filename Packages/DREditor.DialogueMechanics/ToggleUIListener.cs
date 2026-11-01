//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    public class ToggleUIListener : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isUIToggled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<GameObject> gameObjects = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => isUIToggled.AddListener(SetGameObjectsToState);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => isUIToggled.RemoveListener(SetGameObjectsToState);

        /// <summary>
        ///
        /// </summary>
        /// <param name="activeState"></param>
        protected virtual void SetGameObjectsToState(bool activeState)
        {
            foreach (GameObject go in gameObjects)
                if (go != null)
                    go.SetActive(activeState);
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => SetGameObjectsToState(true);
    }
}