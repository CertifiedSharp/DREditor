//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SpotSelectionDisplayer) + ", " + nameof(SpotSelectionComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class SpotSelectionImageDisplayer : SpotSelectionDisplayer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image CG = null;


        /// <inheritdoc/>
        public override void Initialize(SpotSelectionBuilder builder)
        {
            CG.sprite = builder.sprite;

            displayer.Show();
        }
    }
}