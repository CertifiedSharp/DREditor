//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhraseTextHandlerBase) + ", " + nameof(PhraseTextBase) + ", " + nameof(IPhraseHittable) +
        ", " + nameof(IPhraseResetter) +
        "\n\n")]
    public class PhraseText : PhraseTextHandlerBase, IPhraseHittable
    {
        /// <summary>
        ///
        /// </summary>
        public UnityEvent<IPhraseShatterable, PhraseBase, Vector3> onHitEvaluate;

        public UnityEvent<IPhraseShatterable, PhraseBase, Vector3> OnHitEvaluate { get => onHitEvaluate; set => onHitEvaluate = value; }


        /// <inheritdoc/>
        public virtual void Hit(Vector3 hitPoint)
        {
            TryGetComponent(out IPhraseShatterable shatterComponent);

            OnHitEvaluate?.Invoke(shatterComponent, textPhrase, hitPoint);
        }


        /// <inheritdoc/>
        public override void Reset()
        {
            base.Reset();

            transform.localPosition = Vector3.zero;

            transform.localEulerAngles = Vector3.zero;
        }
    }
}