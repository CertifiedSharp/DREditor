//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhraseTextHandlerBase) + ", " + nameof(PhraseTextBase) + ", " + nameof(IPhraseResetter) +
        "\n\n")]
    public class WhiteNoiseText : PhraseTextHandlerBase
    {
        /// <inheritdoc/>
        public override void Reset()
        {
            base.Reset();

            transform.localPosition = Vector3.zero;

            transform.localEulerAngles = Vector3.zero;
        }
    }
}