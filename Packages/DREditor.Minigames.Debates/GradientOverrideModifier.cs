//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class GradientOverrideModifier : PhraseModifierBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TMP_ColorGradient gradient;

        /// <summary>
        ///
        /// </summary>
        public TMP_ColorGradient Gradient { get => gradient; set => gradient = value; }


        /// <inheritdoc/>
        public override string DisplayName
            => gradient != null ? base.DisplayName + ": " + gradient.name : base.DisplayName;
    }
}