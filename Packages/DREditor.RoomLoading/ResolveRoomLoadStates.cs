//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomLoadComponent) + ", " + nameof(IRoomLoadResolver) +
        "\n\n")]
    public class ResolveRoomLoadStates : RoomLoadComponent, IRoomLoadResolver
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLoading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inThreeDMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingToRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onLoadingThreeDRoom = null;


        /// <inheritdoc/>
        public virtual void OnRoomLoadResolve() => ResolveStates();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveStates()
        {
            InvokeActiveState();

            DeactivateStates();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeActiveState()
        {
            inTPFD.Invoke();

            if (inThreeDMode.Value)
                onLoadingThreeDRoom.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DeactivateStates()
        {
            isLoadingToRoom.SetValue(false);

            inLoading.SetValue(false);

            inLeaveProcess.SetValue(false);

            inMenu.SetValue(false);
        }
    }
}