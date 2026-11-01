//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISwipeListener) +
        "\n\n")]
    public class CutLimitMechanic : MonoBehaviour, ISwipeListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent cutCount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent outOfCuts;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI cutVisual = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int cutReduceAmount = 1;


        /// <summary>
        ///
        /// </summary>
        /// <param name="swipeStart"></param>
        /// <param name="swipeEnd"></param>
        public virtual void OnSwiped(Vector3 swipeStart, Vector3 swipeEnd)
        {
            if (cutCount == null)
                return;

            cutCount.SetValue(cutCount.Value - cutReduceAmount);

            if (outOfCuts != null)
                outOfCuts.SetValue(cutCount.Value == 0);

            if (cutVisual != null)
                cutVisual.text = cutCount.Value.ToString();
        }
    }
}