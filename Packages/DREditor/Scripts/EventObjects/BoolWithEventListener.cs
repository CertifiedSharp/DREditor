//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class BoolWithEventListener : BoolWithEventListenerBase<BoolWithEvent>
    {
        protected override void Subscribe() => boolWithEvent.AddListener(OnEventChanged);

        protected override void Unsubscribe() => boolWithEvent.RemoveListener(OnEventChanged);
    }
}