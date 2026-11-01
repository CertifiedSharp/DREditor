//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues;
using DREditor.Minigames.Debates.Mechanics;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(PanelCounterBase) + ", " + nameof(DialogueComponent) + ", " + 
        nameof(IPanelLineListener) + ", " + nameof(IDebateUIVisibility) +
        "\n\n")]
    public class PanelCounterSpeaker : PanelCounterBase, IPanelLineListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image emptyPortrait = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage panelPortrait = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text panelSpeaker = null;


        /// <inheritdoc/>
        public virtual void SetCurrentLine(Line currentLine)
        {
            UpdateLineSpeaker(currentLine);

            UpdateLinePortrait(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void UpdateLineSpeaker(Line currentLine)
        {
            if (panelSpeaker != null)
                panelSpeaker.text = currentLine.Speaker.FirstName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void UpdateLinePortrait(Line currentLine)
        {
            if (panelPortrait != null)
                panelPortrait.texture = currentLine.Speaker.NSDPortrait;
        }
        
        /// <inheritdoc/>
        protected override void ChangeVisibility(float to, float duration)
        {
            if (panelSpeaker != null)
                panelSpeaker.text = "";
#if DOTWEEN
            if (emptyPortrait != null)
                emptyPortrait.DOFade(to, duration);

            if (panelPortrait != null)
                panelPortrait.DOFade(to, duration);
#else
            if (emptyPortrait != null)
                emptyPortrait.color = new(emptyPortrait.color.r, emptyPortrait.color.g, emptyPortrait.color.b, to);

            if (panelPortrait != null)
                panelPortrait.color = new(panelPortrait.color.r, panelPortrait.color.g, panelPortrait.color.b, to);
#endif
        }


        /// <inheritdoc/>
        public override IEnumerator WaitForChangeToComplete()
        {
#if DOTWEEN
            yield return new WaitUntil(() => !DOTween.IsTweening(this));
#else
            yield return null;
#endif
        }
    }
}