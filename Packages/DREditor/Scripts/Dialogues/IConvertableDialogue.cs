//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    public interface IConvertableDialogue
    {
        int LineCount { get; }

        Line GetLine(int index);
    }
}
