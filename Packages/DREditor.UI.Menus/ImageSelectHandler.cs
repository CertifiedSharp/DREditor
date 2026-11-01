//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(SelectHandler) +
        "\n\n")]
    [DefaultExecutionOrder(-1)]
    public class ImageSelectHandler : SelectHandler
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
        [FormerlySerializedAs("selectImage")]
        [SerializeField] protected Sprite selectedSprite = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool colorImage = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("initialText")]
        [SerializeField] protected Color initialColor = Color.black;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("selectText")]
        [SerializeField] protected Color selectedColor = Color.white;

        /// <summary>
        ///
        /// </summary>
        [Divider]

        [Tooltip("")]
        [SerializeField] protected bool enableMode = false;


        /// <summary>
        ///
        /// </summary>
        protected Sprite initialSprite = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (!image)
                image = GetComponent<Image>();

            if (!enableMode && image)
                initialSprite = image.sprite;

            if (colorImage)
                initialColor = image.color;
        }

        /// <inheritdoc/>
        public override void Select()
        {
            if (enableMode && !image.enabled)
                image.enabled = true;

            if (!enableMode && image && selectedSprite)
                image.sprite = selectedSprite;

            if (colorImage)
                image.color = selectedColor;
        }

        /// <inheritdoc/>
        public override void Deselect()
        {
            if (enableMode && image.enabled)
                image.enabled = false;

            if (!enableMode && image)
                image.sprite = initialSprite;

            if (colorImage)
                image.color = initialColor;
        }
    }
}