//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class OptionsTextMessage : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent optionsUIMessage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI box = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => optionsUIMessage.AddListener(PlayMessage);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => optionsUIMessage.RemoveListener(PlayMessage);


        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public virtual void PlayMessage(string message)
        {
            ClearBox();

            box.text = message;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearBox() => box.text = "";
    }
}