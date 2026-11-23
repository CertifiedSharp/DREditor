//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    [System.Serializable]
    public class DirectTo : EndEventBase
    {
        public DialogueCore NewDialogue;

        public int NewDialogueIndex;

        public void SetNewDialogueIndex(int NewDialogueIndex) => this.NewDialogueIndex = NewDialogueIndex;

        public void SetNewDialogue(DialogueCore NewDialogue) => this.NewDialogue = NewDialogue;
    }
}
