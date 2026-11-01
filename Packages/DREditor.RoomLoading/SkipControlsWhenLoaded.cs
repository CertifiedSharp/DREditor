//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    public class SkipControlsWhenLoaded : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent skipActivateControls = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedLoadingRoom = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => EvaluateLoadSubscription();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateLoadSubscription()
        {
            if (!isLoadingFile.Value)
            {
                skipActivateControls.SetValue(true);

                onFinishedLoadingRoom.Response.AddListener(OnFinishedLoadingRoom);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnFinishedLoadingRoom()
        {
            onFinishedLoadingRoom.Response.RemoveListener(OnFinishedLoadingRoom);

            skipActivateControls.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => OnFinishedLoadingRoom();
    }
}