//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class RoomLoadFinisher : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutOnLoadRoomOptionTime = 1f;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => onFinishedLoadingRoom.Response.AddListener(Finish);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => onFinishedLoadingRoom.Response.RemoveListener(Finish);


        /// <summary>
        ///
        /// </summary>
        public virtual void Finish()
        {
            ServiceLocator.GetService<RoomChangerBase>().SetDestination(null);

            CheckFadeOutOption();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckFadeOutOption()
        {
            var options = ServiceLocator.GetService<RoomChangerBase>().GetOptions();

            if (options != null && options.blackTransition && options.fadeOutOnLoad)
                ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutOnLoadRoomOptionTime);

            if (!inDialogue.Value && !inTrialMode.Value)
                hasMenuAccess.SetValue(true);
        }
    }
}