//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    public interface ITypeCallback
    {
        void Invoke<T>(DialogueCore dialogue) where T : Line;
    }
}
