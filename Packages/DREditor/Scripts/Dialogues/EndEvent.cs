//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class EndEvent : EndEventBase
    {
        [FormerlySerializedAs("CustomCall")]
        public string customCall;
        public string CustomCall { get => customCall; set => customCall = value; }

        [FormerlySerializedAs("CallBreak")]
        public bool callBreak = false;
        public bool CallBreak { get => callBreak; set => callBreak = value; }
    }
}
