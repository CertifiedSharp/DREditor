//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
using System.Collections;
using UnityEngine.Events;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    public interface IShatterGraphic
    {
        /// <summary>
        ///
        /// </summary>
        UnityEvent OnPreShatter { get; }

        /// <summary>
        ///
        /// </summary>
        UnityEvent OnMeshShattering { get; }

        /// <summary>
        ///
        /// </summary>
        UnityEvent OnPostShatter { get; }

        /// <summary>
        ///
        /// </summary>
        IEnumerator ShatterTheGraphic();
    }
}