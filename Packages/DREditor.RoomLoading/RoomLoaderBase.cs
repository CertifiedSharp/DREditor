//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RoomLoaderBase : ServiceHolder<RoomLoaderBase>, ISceneEventListener, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<SceneEvent> allowRoomsToLoadOn = new();

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected SceneEvent onLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedRoomTransitions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedLoadingRoomNoDialogue = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            foreach(SceneEvent sceneEvent in allowRoomsToLoadOn)
                sceneEvent.RegisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach (SceneEvent sceneEvent in allowRoomsToLoadOn)
                sceneEvent.UnregisterListener(this);
        }


        /// <summary>
        ///
        /// </summary>
        public abstract void AllowRoomsToLoad();

        /// <summary>
        ///
        /// </summary>
        public abstract void DoNotAllowRoomsToLoad();


        /// <inheritdoc/>
        public virtual void OnEventRaised() => AllowRoomsToLoad();

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => DoNotAllowRoomsToLoad();
    }
}