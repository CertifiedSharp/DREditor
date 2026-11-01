//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using DREditor.Processables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Events/Active By Scene Event", fileName = "ActiveBySceneEvents")]
    public class ActiveBySceneEvent : ScriptableObject, IEventInitializable
    {
        /// <summary>
        ///
        /// </summary>
        public bool IsActive => active;

        [Header("")]
        [Tooltip("")]
        [SerializeField] protected bool active = true;

        [Tooltip("")]
        [SerializeField] protected List<SceneEvent> enableEvents = new();

        [Tooltip("")]
        [SerializeField] protected List<SceneEvent> disableEvents = new();

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent sceneEventTrigger = null;


        /// <summary>
        ///
        /// </summary>
        protected SceneEventListenerWrapper enableListener;

        /// <summary>
        ///
        /// </summary>
        protected SceneEventListenerWrapper disableListener;

        /// <summary>
        ///
        /// </summary>
        protected bool initialActiveState = true;

        /// <summary>
        ///
        /// </summary>
        protected bool initialized = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Enable()
        {
            sceneEventTrigger.SetValue(true);

            SetActive(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Disable()
        {
            sceneEventTrigger.SetValue(true);

            SetActive(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        protected virtual void SetActive(bool active) => this.active = active;

        public virtual void Initialize()
        {
            if (initialized || !Application.isPlaying)
                return;

            enableListener = new(Enable);

            disableListener = new(Disable);

            RegisterEvents(enableEvents, enableListener);

            RegisterEvents(disableEvents, disableListener);

            initialized = true;

            initialActiveState = active;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="events"></param>
        /// <param name="listener"></param>
        protected virtual void RegisterEvents(List<SceneEvent> events, ISceneEventListener listener)
        {
            foreach (var sceneEvent in events)
                sceneEvent.RegisterListener(listener);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="events"></param>
        /// <param name="listener"></param>
        protected virtual void UnregisterEvents(List<SceneEvent> events, ISceneEventListener listener)
        {
            foreach (var sceneEvent in events)
                sceneEvent.UnregisterListener(listener);
        }

        public virtual void Resolve()
        {
            if (!initialized)
                return;

            UnregisterEvents(enableEvents, enableListener);

            UnregisterEvents(disableEvents, disableListener);

            enableListener = null;

            disableListener = null;

            initialized = false;

            SetActive(initialActiveState);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Load(object obj) => active = (bool)obj;
    }
}