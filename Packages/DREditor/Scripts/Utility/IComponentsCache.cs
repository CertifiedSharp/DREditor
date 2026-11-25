//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Interface to control a specifically designated cache of behaviours to manipulate multiple tracked components en masse.
    /// </summary>
    public interface IComponentsCache
    {
        /// <summary>
        /// A specifically designated cache of behaviours to manipulate multiple tracked components en masse.
        /// </summary>
        MonoBehaviour[] CachedComponents { get; }
    }
}