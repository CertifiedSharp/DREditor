//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueSplitter : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Dialogue DialogueToSplit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int splitAtLine = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string targetDirectory = "";


        public DialogueSplitter()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);

            MethodInfo getActiveFolderPath = projectWindowUtilType
                .GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

            object obj = getActiveFolderPath.Invoke(null, new object[0]);

            string pathToCurrentFolder = obj.ToString();

            pathToCurrentFolder = pathToCurrentFolder.Replace("Assets/", "");

            targetDirectory = pathToCurrentFolder;

            UnityEngine.Debug.Log(pathToCurrentFolder);
        }


        [MenuItem("DREditor/Dialogue/Split Dialogues")]
        public static void CreateWizard() => DisplayWizard<DialogueSplitter>("Split Dialogue", "Split");

        protected virtual void OnWizardCreate() => CreateSplitDialogue();


        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateSplitDialogue()
        {
            if (splitAtLine == 0)
            {
                UnityEngine.Debug.LogWarning("You cannot split at the first line!");

                return;
            }

            Dialogue currentDialogue = CreateInstance<Dialogue>();

            CopySplitLines(currentDialogue);

            CopyDirectTo(currentDialogue);

            CreateSplitAsset(currentDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDialogue"></param>
        protected virtual void CopySplitLines(Dialogue currentDialogue)
        {
            for (int i = splitAtLine; i < DialogueToSplit.Lines.Count; i++)
            {
                Line line = DialogueToSplit.Lines[i];

                line = CustomEditorUtility.DeepCopyJson(line);

                currentDialogue.Lines.Add(line);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDialogue"></param>
        protected virtual void CopyDirectTo(Dialogue currentDialogue)
        {
            if (DialogueToSplit.DirectTo.Enabled)
            {
                currentDialogue.SetDirectTo(new DirectTo<Line>());

                currentDialogue.DirectTo.SetEnabled(true);

                currentDialogue.DirectTo.SetNewDialogue(DialogueToSplit.DirectTo.NewDialogue);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDialogue"></param>
        protected virtual void CreateSplitAsset(Dialogue currentDialogue)
        {
            currentDialogue.name = DialogueToSplit.name;

            string folder = FolderUtility.CreateIntermediateFolders(targetDirectory);

            AssetDatabase.StartAssetEditing();

            string diaPath = folder + "/" + currentDialogue.name + "_Split.asset";

            AssetDatabase.CreateAsset(currentDialogue, diaPath);

            AssetDatabase.StopAssetEditing();

            AssetDatabase.SaveAssets();
        }
    }
#endif
}