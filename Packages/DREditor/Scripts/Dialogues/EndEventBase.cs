//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public abstract class EndEventBase : IEndEvent
    {
        [HideInInspector]
        public bool Enabled;

        public virtual bool IsValid() => true;

        public virtual void SetEnabled(bool Enabled) => this.Enabled = Enabled;
    }
}
