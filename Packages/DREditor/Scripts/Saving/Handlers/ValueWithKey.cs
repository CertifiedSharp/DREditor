//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TY"></typeparam>
    [Serializable]
    public class ValueWithKey<T, TY> where TY : UnityEvent<T>, new()
    {
        public ValueWithKey(string key)
        {
            this.key = key;
        }

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string key;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public ValueWithEvent<T, TY> value;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool skipOnChapterSelect = false;
    }
}