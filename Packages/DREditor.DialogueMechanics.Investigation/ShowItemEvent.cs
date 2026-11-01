//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.EventObjects;
using DREditor.UIHandling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class ShowItemEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage image = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityUIComponent = null;


        [Header("")]
        [SerializeField] protected UnityEvent onItemShown;

        [Header("")]
        [SerializeField] protected UnityEvent onItemHidden;


        /// <summary>
        ///
        /// </summary>
        private void Awake() => SetDefaultEventString("ShowItem");

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (o != null)
                DisplayItem((Texture2D)o);
            else
                visibilityUIComponent.SetVisibility(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tex"></param>
        protected virtual void DisplayItem(Texture2D tex)
        {
            bool showItem = tex != null;

            if (showItem)
                image.texture = tex;

            visibilityUIComponent.SetVisibility(showItem);

            var visibilityEvent = showItem ? onItemShown : onItemHidden;

            if (!isLoadingFile.Value)
                visibilityEvent?.Invoke();
        }
    }
}