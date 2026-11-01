//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;

namespace DREditor.Editor.Toolbox
{
    public interface IDialogueCaptureGroup
    {
        bool IsPriority { get; }

        void Capture<T>(ref T line, string capturedText) where T : Line;
    }
}