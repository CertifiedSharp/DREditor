//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjectServices;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public partial class ChapterUIText : IntWithEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("textO")]
        [SerializeField] protected TextMeshProUGUI chapterText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextUIPositionPair defaultText;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int enableIDX = -1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int disableIDX = -1;

        
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TextUIPositionPair[] texts = null;


        /// <inheritdoc/>
        protected override void OnValueChanged(int updatedValue)
        {
            base.OnValueChanged(updatedValue);

            ChangeUI(updatedValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        protected virtual void ChangeUI(int chapterIndex)
        {
            if (chapterIndex >= 0 && chapterIndex < texts.Length)
                SetTextPair(texts[chapterIndex]);
            else
                SetTextPair(defaultText);

            if (chapterIndex == enableIDX)
                chapterText.enabled = true;

            if (chapterIndex == disableIDX)
                chapterText.enabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pair"></param>
        protected virtual void SetTextPair(TextUIPositionPair pair)
        {
            chapterText.text = pair.text;

            chapterText.gameObject.transform.localPosition = pair.position;
        }
    }
}