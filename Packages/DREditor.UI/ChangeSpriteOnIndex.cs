//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class ChangeSpriteOnIndex : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image image = null;

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


        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        public virtual void ChangeUI(int index)
        {
            if (index == enableIDX)
                image.enabled = true;

            if (index == disableIDX)
                image.enabled = false;

            if (index < sprites.Length)
                image.sprite = sprites[index] != null ? sprites[index] : defaultSprite;
        }
    }
}