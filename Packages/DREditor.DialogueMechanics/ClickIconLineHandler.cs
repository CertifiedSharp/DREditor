//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Gardenusing DREditor.Dialogues;
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + 
        nameof(ILinePostTextDisplay) + ", " + nameof(ILineEndProcess) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ClickIconLineHandler : DialogueComponent, ILinePostTextDisplay, ILineEndProcess, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inAutoMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent clickIconVisible = null;


        /// <inheritdoc/>
        public virtual void LinePostTextProcess(Line currentLine)
        {
            if (!inAutoMode.Value)
                clickIconVisible.SetValue(true);
        }

        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null)
        {
            if (!inAutoMode.Value)
                clickIconVisible.SetValue(false);
        }


        protected virtual void OnEnable() => inAutoMode.AddListener(EnsureClickVisibilityOnStoppingAuto);

        protected virtual void OnDisable() => inAutoMode.RemoveListener(EnsureClickVisibilityOnStoppingAuto);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inAuto"></param>
        protected virtual void EnsureClickVisibilityOnStoppingAuto(bool inAuto)
        {
            if (!inAuto && !clickIconVisible.Value)
                clickIconVisible.Invoke();
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => clickIconVisible.SetValue(false);
    }
}