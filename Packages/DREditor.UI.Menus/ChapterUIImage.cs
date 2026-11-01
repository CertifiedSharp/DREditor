//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjectServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class ChapterUIImage : IntWithEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("image")]
        [SerializeField] protected Image chapterImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite defaultSprite = null;


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
        [SerializeField] protected Sprite[] sprites = null;


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
            if (chapterIndex == enableIDX)
                chapterImage.enabled = true;

            if (chapterIndex == disableIDX)
                chapterImage.enabled = false;

            if (chapterIndex < sprites.Length)
                chapterImage.sprite = sprites[chapterIndex] != null ? sprites[chapterIndex] : defaultSprite;
        }
    }
}