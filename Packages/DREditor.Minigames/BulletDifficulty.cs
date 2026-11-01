//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using DREditor.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class BulletDifficulty
    {
        /// <summary>
        ///
        /// </summary>
        public StateAsset difficulty = null;

        /// <summary>
        ///
        /// </summary>
        public List<TruthBullet> bullets = new();


        public BulletDifficulty() { }

        public BulletDifficulty(StateAsset difficulty) => this.difficulty = difficulty;


        /// <summary>
        ///
        /// </summary>
        public virtual void RemoveDuplicateBullets() => bullets = new HashSet<TruthBullet>(bullets).ToList();
    }
}