//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
using System.Linq;
#endif
using DREditor.Dialogues;
using DREditor.Minigames.Debates.Mechanics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PanelCounterBase) + ", " + nameof(DialogueComponent) + ", " +
        nameof(IPanelCounter) + ", " + nameof(IDebateUIVisibility) +
        "\n\n")]
    public class PanelCounterIndicator : PanelCounterBase, IPanelCounter
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite notTalking = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite talking = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image[] panelCounters = new Image[20];


        /// <summary>
        ///
        /// </summary>
        protected int panelCount = 0;


        /// <inheritdoc/>
        public override void Show()
        {
            Restart();

            base.Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Restart()
        {
            for (int i = 0; i < panelCounters.Length; i++)
                panelCounters[i].sprite = notTalking;
        }


        /// <inheritdoc/>
        protected override void ChangeVisibility(float to, float duration)
        {
            for (int i = 0; i < panelCount && i < panelCounters.Length; i++)
#if DOTWEEN
                panelCounters[i].DOFade(to, duration);
#else
                panelCounters[i].color = new(panelCounters[i].color.r, panelCounters[i].color.g, panelCounters[i].color.b, to);
#endif
        }


        /// <inheritdoc/>
        public virtual void NextPanelCounter(int i)
        {
            if (i >= panelCounters.Length || panelCount >= panelCounters.Length)
                return;

            if (i != 0)
            {
                panelCounters[i - 1].sprite = notTalking;

                panelCounters[i].sprite = talking;
            }
            else
            {
                panelCounters[panelCount - 1].sprite = notTalking;

                panelCounters[i].sprite = talking;
            }
        }


        /// <inheritdoc/>
        public virtual void InitializePanelCount(int panelCount) => this.panelCount = panelCount;


        /// <inheritdoc/>
        public override IEnumerator WaitForChangeToComplete()
        {
#if DOTWEEN
            yield return new WaitUntil(() => !DOTween.IsTweening(panelCounters.Any()));
#else
            yield return null;
#endif
        }
    }
}