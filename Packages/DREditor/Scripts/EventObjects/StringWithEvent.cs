//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.EventObjects
{
    [CreateAssetMenu(menuName = "DREditor/Event Objects/String", fileName = "New StringWithEvent")]
    public class StringWithEvent : ValueWithEvent<string, StringEvent>
    {
        public string Resolve()
        {
            return Value;
        }
    }

    [Serializable]
    public class StringVariable : CachedVariable<StringWithEvent, string, StringEvent> { }
}