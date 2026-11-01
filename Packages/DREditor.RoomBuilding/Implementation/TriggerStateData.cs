//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Triggers;
using System;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class TriggerStateData : StateDataAsset<EventTrigger>
    {
#if UNITY_EDITOR
        [ContextMenu("Convert")]
        public void Convert() => TriggerConverter.ConvertTriggerAsset(this);
#endif
    }
}