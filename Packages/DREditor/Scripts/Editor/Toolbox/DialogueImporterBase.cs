//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    [Serializable]
    public abstract class DialogueImporterBase : IDialogueImporter
    {
        protected string line;

        protected long bytesRead = 0;

        protected long totalBytes;

        /// <summary>
        ///
        /// </summary>
        protected readonly double WaitForProgress = 0.5;

        public abstract bool ImporterValid();

        public virtual void Import<T, T2>(List<T> dialoguesToSave, TextReader reader, long totalBytes)
            where T : DialogueCore
            where T2 : Line, new()
        {
            if (!ImporterValid())
                return;

            this.totalBytes = totalBytes;

            T currentDialogue = AddNewDialogueToSave(dialoguesToSave, InitializeImport);

            while (reader.ReadLine() is string line)
            {
                InitializeLineRead(line);

                IncreaseBytesRead();

                if (ImportedDialogueLine<T, T2>(dialoguesToSave, ref currentDialogue))
                    UpdateProgressBar();
                else
                    break;
            }

            OnImportComplete();
        }

        protected virtual T AddNewDialogueToSave<T>(List<T> dialoguesToSave, Action action)
            where T : DialogueCore
        {
            T currentDialogue = CreateNewDialogue<T>();

            dialoguesToSave.Add(currentDialogue);

            action();

            return currentDialogue;
        }

        protected virtual T CreateNewDialogue<T>() where T : DialogueCore => ScriptableObject.CreateInstance<T>();

        protected abstract void InitializeImport();

        protected virtual void InitializeLineRead(string line) => this.line = line;

        protected virtual void IncreaseBytesRead() => bytesRead += Encoding.UTF8.GetByteCount(line);

        protected abstract bool ImportedDialogueLine<T, T2>(List<T> dialoguesToSave, ref T currentDialogue)
            where T : DialogueCore where T2 : Line, new();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void UpdateProgressBar()
        {
            if (EditorApplication.timeSinceStartup > WaitForProgress)
                EditorUtility.DisplayProgressBar("Processing...", "Procesing text file.", bytesRead / totalBytes * 0.9f);
        }

        protected abstract void OnImportComplete();
    }
}