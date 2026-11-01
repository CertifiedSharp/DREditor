//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Minigames.Debates.Absorbing
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [MovedFrom(true, "DREditor.EG", "EG")]
    public class PathosAnswerModifier : PhraseModifierBase, IPathosAnswer
    {
        /// <summary>
        ///
        /// </summary>
        [Header(nameof(IPathosAnswer) + "")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string pathosAnswer;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int pathosSequenceSkips;


        public virtual string PathosAnswer { get => pathosAnswer; set => pathosAnswer = value; }

        public virtual int PathosSequenceSkips { get => pathosSequenceSkips; set => pathosSequenceSkips = value; }
    }
}