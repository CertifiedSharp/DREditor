//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class StateChangeGroup<T>
    {
        public StateChangeGroup(StateAsset state = null) => this.state = state;


        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public StateAsset state;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public T stateValue;
    }
}