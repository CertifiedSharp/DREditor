//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MatchBoolEventState
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent boolEvent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("state")]
        [SerializeField] protected bool matchState = false;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool Matches() => boolEvent.Value == matchState;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string LogState() => boolEvent.name + ": " + boolEvent.Value + " Must be: " + matchState;
    }
}