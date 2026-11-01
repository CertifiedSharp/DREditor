// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ShatterProcessBase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        protected bool ready;


        /// <summary>
        ///
        /// </summary>
        public virtual void Ready() => ready = true;

        /// <summary>
        ///
        /// </summary>
        public virtual void Unready() => ready = false;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract ShatterShape[] GetShapes();

        /// <summary>
        ///
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="hitPoint"></param>
        /// <param name="mesh"></param>
        public abstract void Shatter(MonoBehaviour runner, Vector3 hitPoint, Mesh mesh);

        /// <summary>
        ///
        /// </summary>
        /// <param name="updateCallback"></param>
        /// <returns></returns>
        public abstract IEnumerator UpdateShatter(Action<Vector3[], Color[]> updateCallback);
    }
}