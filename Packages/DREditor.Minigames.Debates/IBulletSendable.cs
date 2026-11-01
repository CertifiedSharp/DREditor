//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using System.Collections.Generic;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="bullets"></param>
        void OnSentBullets(List<TruthBullet> bullets);
    }
}