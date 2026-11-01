//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TutorialComponentBase) +
        "\n\n")]
    public class TutorialTitleText : TutorialComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text title = null;


        /// <inheritdoc/>
        public override void Initialize(TutorialAsset asset)
        {
            if (title != null)
                title.text = asset.title;
        }


        /// <inheritdoc/>
        public override void Resolve()
        {
            if (title != null)
                title.text = "";
        }
    }
}