//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DialogueBased<T> : DialogueBase<T> where T : Line, new()
    {
        /// <inheritdoc/>
        public override void InvokeDialogue(ITypeCallback callback) => callback?.Invoke<T>(this);
    }
}
