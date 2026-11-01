// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IClosingArgumentListener) +
        "\n\n")]
    public class CorrectPanelUpdater : MonoBehaviour, IClosingArgumentListener
    {
        /// <inheritdoc/>
        public virtual void OnStockCorrect(Transform hoveredObject)
        {
            if (hoveredObject.TryGetComponent(out QuestionPanelBase panel))
                panel.OnPanelCorrect();
        }


        /// <inheritdoc/>
        public virtual void CAReset() { }
    }
}