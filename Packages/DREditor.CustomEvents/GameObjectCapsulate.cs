//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using System;
using DREditor.Dialogues.Events;

#if UNITY_EDITOR
using DREditor.Editor.Utility;
#endif

namespace DREditor.CustomEvents
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class GameObjectCapsulate : IDataCapsulate
    {
        /// <summary>
        ///
        /// </summary>
        public GameObject value;

#if UNITY_EDITOR
        /// <inheritdoc/>
        public virtual void Display(object o = null) => value = HandyFields.UnityField(value);
#else
        public virtual void Display(object o = null)
        {

        }
#endif
    }
}