//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden\
#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TransformUIVisibility) + ", " + nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class MoveUIVisibility : TransformUIVisibility
    {
#if DOTWEEN
        /// <inheritdoc/>
        protected override Tween CreateTween(Vector3 to, float duration) => target.DOMove(target.position + to, duration);
#endif
    }
}