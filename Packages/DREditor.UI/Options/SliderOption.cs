//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
using System;
using UnityEngine;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Event Objects/EventOptions/Slider", fileName = "New Slider Option")]
    public class SliderOption : OptionWithEvent<float, FloatEvent>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public float min;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public float max;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public float value;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public SceneEvent onSliderSettingChanged;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool invokeTextOnSliderChanged = false;
    }
}