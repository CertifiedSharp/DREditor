//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Minigames.Debates;
using UnityEngine;

namespace DREditor.Minigames.NSD.Managers
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/NSD", fileName = "NSD Builder")]
    public class NSDBuilder : DebateBuilder<Panel>
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected DialogueCore endDialogue;

        /// <summary>
        ///
        /// </summary>
        public DialogueCore EndDialogue { get => endDialogue; set => endDialogue = value; }


        /// <inheritdoc/>
        public override void Convert()
        {
            foreach (var panel in Lines)
                foreach (var phrase in panel.Phrases)
                    phrase.Convert();
        }

        /// <inheritdoc/>
        public override bool Scan(out string log)
        {
            log = "";

            bool lineIssue = false;

            if (EndDialogue == null)
                log += $"\n- Does not have End Dialogue!";

            foreach (var panel in Lines)
                if (panel is IDebateScanner scannable && scannable.Scan(out string lineResult))
                {
                    log += $"\n- Issue on Panel index: {Lines.IndexOf(panel)} {lineResult}";

                    lineIssue = true;
                }

            log = log != "" ? name + " contains issues: " + log : log;

            return EndDialogue == null || lineIssue;
        }
    }
}