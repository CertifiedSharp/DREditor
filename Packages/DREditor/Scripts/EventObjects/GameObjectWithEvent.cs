//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.EventObjects
{

    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Event Objects/GameObject", fileName = "New GameObject")]
    public class GameObjectWithEvent : ValueWithEvent<GameObject, GameObjectEvent>
    {
        public bool Resolve()
        {
            return true;
        }
    }
}