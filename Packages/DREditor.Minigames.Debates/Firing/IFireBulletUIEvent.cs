//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    public interface IFireBulletUIEvent : IFireBulletUI
    {
        /// <summary>
        ///
        /// </summary>
        public UnityEvent<TruthBullet> OnUIBulletFired { get; set; }
    }
}