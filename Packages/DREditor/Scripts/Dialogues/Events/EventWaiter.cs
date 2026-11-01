//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.Events
{
    [Serializable]
    public class EventWaiter
    {
        public bool done = false;

        public void SetUnfinished() => done = false;
        public void SetFinished() => done = true;
    }
}