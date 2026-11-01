//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ICycleUIStateComponent) +
        "\n\n")]
    public class CycleUIIndenter : MonoBehaviour, ICycleUIStateComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 indentOffset = new(75, 20, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float duration = 0.1f;


        /// <inheritdoc/>
        public virtual void DisplayBulletActive(bool isActive)
        {
#if DOTWEEN
            Vector3 newPosition = transform.localPosition + (isActive ? indentOffset : indentOffset * -1);

            transform.DOLocalMove(newPosition, duration)
                .SetUpdate(true);
#endif
        }
    }
}