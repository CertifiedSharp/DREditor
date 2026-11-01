//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public class SetTextDisplaySpeed : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextDisplayerBase textDisplayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ValueWithEvent<float, FloatEvent> textSpeedSetting = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => OnTextDisplaySpeedUpdated(textSpeedSetting.Value);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => SubscribeToSettingChange();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToSettingChange() => textSpeedSetting.AddListener(OnTextDisplaySpeedUpdated);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnsubscribeToSettingChange();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToSettingChange() => textSpeedSetting.RemoveListener(OnTextDisplaySpeedUpdated);

        /// <summary>
        ///
        /// </summary>
        /// <param name="newSpeed"></param>
        protected virtual void OnTextDisplaySpeedUpdated(float newSpeed)
        {
            if (textDisplayer != null)
                textDisplayer.SetTextDisplaySpeed(newSpeed);
        }
    }
}