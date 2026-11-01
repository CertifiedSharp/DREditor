//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhraseShatterable : IPhraseResetter
    {
        /// <summary>
        ///
        /// </summary>
        public UnityEvent<IPhraseShatterable> OnShattered { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitPoint"></param>
        void Shatter(Vector3 hitPoint);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool IsShattering();
    }

}