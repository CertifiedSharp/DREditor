//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;

namespace DREditor.Editor.Toolbox
{
    [Serializable]
    public class TextCaptureGroup : IDialogueCaptureGroup
    {
        public virtual bool IsPriority => false;

        public virtual void Capture<T>(ref T line, string capturedText) where T : Line => line.SetText(capturedText);
    }
}