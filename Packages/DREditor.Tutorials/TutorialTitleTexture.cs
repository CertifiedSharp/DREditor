//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TutorialComponentBase) +
        "\n\n")]
    public class TutorialTitleTexture : TutorialComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image title = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setNativeSize = true;


        /// <inheritdoc/>
        public override void Initialize(TutorialAsset asset)
        {
            if (title != null)
                title.sprite = asset.titleSprite;

            if (setNativeSize)
                title.SetNativeSize();
        }


        /// <inheritdoc/>
        public override void Resolve()
        {
            if (title != null)
                title.sprite = null;
        }
    }
}