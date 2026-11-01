//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class StartInvestigation : EndEventBase
    {
        [FormerlySerializedAs("NextObjective")]
        public bool nextObjective = true;
        public bool NextObjective { get => nextObjective; set => nextObjective = value; }

        [FormerlySerializedAs("NewDialogue")]
        public Dialogue newDialogue;
        public Dialogue NewDialogue { get => newDialogue; set => newDialogue = value; }

        [FormerlySerializedAs("ID")]
        public int id;
        public int ID { get => id; set => id = value; }
    }
}
