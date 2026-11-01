//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(SceneEventListener) + 
        "\n\n")]
    public class SetControlsKeyByMode : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Space]
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] private List<ModeKeyPair> modePairs = new();


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(OnSetControlsByMode);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(OnSetControlsByMode);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSetControlsByMode()
        {
            foreach (var pair in modePairs)
                if (pair.mode.Value)
                    currentControlsKey.SetValue(pair.key);
        }
    }
}