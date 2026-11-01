//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

using System.Collections.Generic;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class BoolsWithEventListener : BoolWithEventListenerBase<List<BoolWithEvent>>
    {
        protected override void Subscribe()
        {
            foreach (var eventInstance in boolWithEvent)
                if (eventInstance != null)
                    eventInstance.AddListener(OnEventChanged);
        }

        protected override void Unsubscribe()
        {
            foreach (var eventInstance in boolWithEvent)
                if (eventInstance != null)
                    eventInstance.RemoveListener(OnEventChanged);
        }
    }
}