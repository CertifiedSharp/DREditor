//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.EventObjects
{
    [CreateAssetMenu(menuName = "DREditor/Event Objects/SelectableObject", fileName = "New SelectableObjectWithEvent")]
    public class SelectableObjectWithEvent : ValueWithEvent<SelectableObject, SelectableObjectEvent>
    {
        public virtual bool Resolve() => Value;
    }
}
