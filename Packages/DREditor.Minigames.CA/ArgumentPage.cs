// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ArgumentPage
    {
        /// <summary>
        ///
        /// </summary>
        public Sprite pageSprite;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("stock")]
        public List<ArgumentQuestionPanel> questionPanels = new();

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageSprite"></param>
        public virtual void SetPageSprite(Sprite pageSprite) => this.pageSprite = pageSprite;

        /// <summary>
        ///
        /// </summary>
        public virtual void AddNewPanel() => questionPanels.Add(new());
    }
}