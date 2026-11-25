//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Potentially removing EndEventBase.
    */
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public abstract class EndEventBase : IEndEvent
    {
        /// <summary>
        /// 
        /// </summary>
        [HideInInspector]
        public bool Enabled;


        /// <inheritdoc/>
        public virtual bool IsValid() => true;
    }
}
