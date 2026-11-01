//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    public abstract class InterjectHandlerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="interjectionObject"></param>
        /// <param name="skipEarly"></param>
        public abstract IEnumerator InterjectionProcess(GameObject interjectionObject, bool skipEarly = false);
    }
}