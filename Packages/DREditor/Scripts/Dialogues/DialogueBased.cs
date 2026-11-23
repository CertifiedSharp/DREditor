//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    public class DialogueBased<T> : DialogueBase<T> where T : Line, new()
    {
        public override void InvokeDialogue(ITypeCallback callback) => callback?.Invoke<T>(this);
    }
}
