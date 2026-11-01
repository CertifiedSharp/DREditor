//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections.Generic;
using System.IO;

namespace DREditor.Editor.Toolbox
{
    public interface IDialogueImporter
    {
        bool ImporterValid();

        void Import<T, T2>(List<T> dialoguesToSave, TextReader reader, long totalBytes)
            where T : DialogueCore where T2 : Line, new();
    }
}