//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using System;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public abstract class DialogueDebugToolEditorBase
    {
        public abstract bool DebugMode(DialogueCore dia);
    }
}
#endif