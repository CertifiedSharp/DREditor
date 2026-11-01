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
    public class RotateUIVisibility : TransformUIVisibility
    {
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RotateMode rotateMode = RotateMode.FastBeyond360;


        /// <inheritdoc/>
        protected override Tween CreateTween(Vector3 to, float duration) => 
            target.DOLocalRotate(target.localEulerAngles + to, duration, rotateMode);
#endif
    }
}