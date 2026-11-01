//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.Services;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RoomChangerBase : ServiceHolder<RoomChangerBase>, ISceneEventListener, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onStartedChangingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent startedNewGame = null;


        /// <summary>
        ///
        /// </summary>
        protected RoomLoadOptionsAsset options;

        /// <summary>
        ///
        /// </summary>
        protected Gate destination = null;


        protected virtual void OnEnable() => startedNewGame.RegisterListener(this);

        protected virtual void OnDisable() => startedNewGame.UnregisterListener(this);

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadRoomOptions"></param>
        public virtual void SetOptions(ScriptableObject loadRoomOptions) => options = (RoomLoadOptionsAsset)loadRoomOptions;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual RoomLoadOptionsAsset GetOptions() => options;

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        public virtual void SetDestination(Gate destination) => this.destination = destination;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Gate GetDestination() => destination;

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="options"></param>
        public abstract void ChangeRoom(Gate destination, ScriptableObject options = null);

        /// <summary>
        ///
        /// </summary>
        public abstract void AssignDestinationPosition();


        /// <inheritdoc/>
        public virtual void OnEventRaised() => SetDestination(null);

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => SetOptions(null);
    }
}