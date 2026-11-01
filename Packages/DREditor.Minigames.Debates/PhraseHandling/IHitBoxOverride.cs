//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    public interface IHitBoxOverride
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="addedBoxes"></param>
        /// <returns></returns>
        Vector3 SetHitBox(PhraseBase phrase, List<BoxCollider> addedBoxes);
    }
}