//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using System;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class BulletAnswerModifier : PhraseModifierBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TruthBullet answerBullet;

        /// <summary>
        ///
        /// </summary>
        public TruthBullet AnswerBullet { get => answerBullet; set => answerBullet = value; }
    }
}