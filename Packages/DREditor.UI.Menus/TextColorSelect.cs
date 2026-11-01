//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(SelectHandler) +
        "\n\n")]
    public class TextColorSelect : SelectHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI tmpText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color initialColor = Color.black;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color selectedColor = Color.white;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (tmpText == null && !TryGetComponent(out tmpText) && !GetComponentInChildren(out tmpText))
                UnityEngine.Debug.LogError($"{ToString()}: " + gameObject.name + ": COULD NOT FIND ASSIGNABLE TEXT");

            initialColor = tmpText.color;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tmpText"></param>
        /// <returns></returns>
        protected virtual bool GetComponentInChildren(out TextMeshProUGUI tmpText)
        {
            var children = GetComponentInChildren<TextMeshProUGUI>();

            tmpText = children;

            return tmpText != null;
        }

        /// <inheritdoc/>
        public override void Select()
        {
            if (tmpText)
                tmpText.color = selectedColor;
        }

        /// <inheritdoc/>
        public override void Deselect()
        {
            if (tmpText)
                tmpText.color = initialColor;
        }
    }
}