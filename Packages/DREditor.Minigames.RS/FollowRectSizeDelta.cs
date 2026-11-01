//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
	[ClassHeader("")]
    public class FollowRectSizeDelta : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RectTransform maskToFollow;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 offset;


        /// <summary>
        ///
        /// </summary>
        protected RectTransform rectTransform;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            if (maskToFollow == null && ServiceLocator.HasService<FollowableRect>())
                maskToFollow = ServiceLocator.GetService<FollowableRect>().rectTransform;

            if (maskToFollow == null)
                UnityEngine.Debug.LogError("There is no mask to follow!");

            rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Update() => FollowRectTransform();

        /// <summary>
        ///
        /// </summary>
        protected virtual void FollowRectTransform()
        {
            if (rectTransform != null && maskToFollow != null)
                rectTransform.localPosition = new Vector3(
                    maskToFollow.localPosition.x + maskToFollow.sizeDelta.x + offset.x,
                    rectTransform.localPosition.y,
                    0);
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Report Mask Positions To Console")]
        public virtual void ReportMaskPositionsToConsole()
        {
            UnityEngine.Debug.Log("REPORT: Follow Mask: " + maskToFollow.sizeDelta.x);

            UnityEngine.Debug.Log("REPORT: Follow Mask: " + maskToFollow.localPosition.x);
        }
    }
}