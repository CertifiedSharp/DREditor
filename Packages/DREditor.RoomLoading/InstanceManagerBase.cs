//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class InstanceManagerBase : RoomLoadComponent, IRoomLoadRoutine, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent startedNewGame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool dontDestroyOnLoad = true;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent loadProgressionData = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent onRoomInstanceLoaded = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => ServiceInitializer.Initialize<InstanceManagerBase>(this, dontDestroyOnLoad);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => startedNewGame.RegisterListener(this);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => startedNewGame.UnregisterListener(this);


        /// <summary>
        ///
        /// </summary>
        public abstract void ClearData();

        /// <summary>
        ///
        /// </summary>
        public abstract void ClearRoomData();

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public virtual bool ContainsRoomDataFor(string roomName) => GetIndexOfRoom(roomName) != -1;

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        protected abstract int GetIndexOfRoom(string roomName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        public abstract void LoadRoomInstance(string roomName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        public abstract void SaveRoomInstance(string roomName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LoadRoomInstance()
        {
            LoadRoomInstance(SceneManager.GetActiveScene().name);

            yield return new WaitUntil(() => onRoomInstanceLoaded.Value);

            UnityEngine.Debug.Log($"{ToString()}: Finished Loading Room Instance.");

            onRoomInstanceLoaded.SetValue(false);

            yield break;
        }


        /// <inheritdoc/>
        public abstract IEnumerator OnRoomLoadRoutine();

        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            ClearData();

            ClearRoomData();
        }
    }
}