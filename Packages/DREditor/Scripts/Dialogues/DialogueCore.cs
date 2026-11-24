//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues
{
    public abstract class DialogueCore : ScriptableObject, IConvertableDialogue
    {
        public string DialogueName = "";

        public static event Action<DialogueCore, Line> OnDebugLine;

        public static void InvokeDebugLine(DialogueCore dialogue, Line line) => OnDebugLine?.Invoke(dialogue, line);

        public abstract int LineCount { get; }

        public abstract Line GetLine(int index);

        public abstract List<T> GetLines<T>();

        public void SetName(string DialogueName) => this.DialogueName = DialogueName;

        public abstract void InvokeDialogue(ITypeCallback callback);

        public abstract void ConvertEndEvents();

        public abstract bool IsNested(DialogueCore dia, List<DialogueCore> history);

#if UNITY_EDITOR
        [HideInInspector]
        public CharacterDatabase Speakers;


        public void SetSpeakers(CharacterDatabase Speakers) => this.Speakers = Speakers;

        public void LoadSpeakers()
        {
            if (Speakers == null)
                SetSpeakers(Resources.Load<CharacterDatabase>("Characters/CharacterDatabase"));
        }

        public string[] GetCharacterNames()
        {
            LoadSpeakers();

            return Speakers.GetNames().ToArray();
        }

        public int[] GetIntValues(int count)
        {
            int[] values = new int[count];

            for (int i = 0; i < values.Length; i++)
                values[i] = i;

            return values;
        }
#endif
    }
}
