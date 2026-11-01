//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueStarterDebugger) + 
        "\n\n")]
    [RequireComponent(typeof(DialogueStarter))]
    public class DialogueAssetStarterDebugger : DialogueStarterDebugger
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="DebugNoDialogue"></param>
        public override void DebugNoCharacterDialogueCall(SelectableObject actor, DialogueCore DebugNoDialogue = null)
        {
            Dialogue dialogueDebug = (Dialogue)DebugNoDialogue;

            Dialogue[] Conversation = new Dialogue[1];

            Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();

            dialogue.Lines.Add(new Line());

            dialogue.Lines[0].SetText("THIS CHARACTER HAS NO DIALOGUE PLUGGED IN DO NOT SAVE ON THIS LINE!");

            Conversation[0] = dialogue;

            if (DebugNoDialogue != null)
                Conversation[0] = dialogueDebug;

            GetComponent<DialogueStarter>().StartDialogue(actor, Conversation);
        }
    }
}