//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<FollowableRect>) +
        "\n\n")]
    [RequireComponent(typeof(RectTransform))]
    public class FollowableRect : ServiceHolder<FollowableRect>
    {
        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public RectTransform rectTransform;

        /// <inheritdoc/>
        protected override void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            base.Awake();
        }
    }
}