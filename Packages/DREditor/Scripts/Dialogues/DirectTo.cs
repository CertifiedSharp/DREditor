//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    [System.Serializable]
    public class DirectTo<T2> : EndEventBase
    {
        public DialogueBase<T2> NewDialogue;
        public int NewDialogueIndex;
        public DialogueBase<T2> NewTrialDialogue;

        public void SetNewDialogueIndex(int NewDialogueIndex) => this.NewDialogueIndex = NewDialogueIndex;

        public void SetNewDialogue(DialogueBase<T2> NewDialogue) => this.NewDialogue = NewDialogue;

        public void SetNewTrialDialogue(DialogueBase<T2> NewTrialDialogue) => this.NewTrialDialogue = NewTrialDialogue;

        public void Disable()
        {
            SetEnabled(false);

            SetNewDialogue(null);

            SetNewTrialDialogue(null);
        }
    }
}
