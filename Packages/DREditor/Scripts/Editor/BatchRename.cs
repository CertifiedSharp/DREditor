//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class BatchRename : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string baseName = "MyObject_";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int startIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int increment = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool decrease = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool addToFront = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string nameForFront = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool setDirectTo = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool removeDirectTo = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool replaceNames = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string replaceString = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string replaceWith = "";

        /// <summary>
        ///
        /// </summary>
        [TextArea]
        [Tooltip("")]
        public string textArea = "";


        [MenuItem("DREditor/Utility/Batch Rename", false, 3)]
        static void CreateWizard() => DisplayWizard("Batch Renamer", typeof(BatchRename), "Rename");

        protected virtual void OnEnable() => UpdateMessage();

        protected virtual void OnSelectionChange() => UpdateMessage();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateMessage()
        {
            helpString = "";

            if (Selection.objects != null)
                helpString = "Selected Objects: " + Selection.objects.Length;
            else
                errorString = "Please Select objects!";

            if (Selection.objects.Length == 0)
                errorString = "No Objects Selected!";
            else
                errorString = "";
        }

        protected virtual void OnWizardCreate()
        {
            if (Selection.objects.Length == 0)
            {
                EditorUtility.DisplayDialog("Must have a selection", "You must have a selection in order to rename.", "OK");

                return;
            }

            if (textArea != "")
            {
                try
                {
                    UseTextArea();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.ToString());
                }
            }
            else if (setDirectTo)
                SetDirectTo();
            else if (replaceNames)
                ReplaceNames();
            else if (!addToFront)
                NotAddToFront();
            else
                AddToFront();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UseTextArea()
        {
            List<string> list = new(textArea.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));

            foreach (Object obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);

                int index = Selection.objects.ToList().IndexOf(obj);

                AssetDatabase.RenameAsset(path, list[index].Replace("\n", ""));

                EditorUtility.SetDirty(obj);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetDirectTo()
        {
            for (int i = 0; i < Selection.objects.Length - 1; i++)
            {
                GenericDirectTo<Line>(i);

                GenericDirectTo<TrialLine>(i);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        protected virtual void GenericDirectTo<T>(int i) where T : Line
        {
            if (Selection.objects[i] is DialogueBase<T> dialogue)
            {
                DialogueBase<T> nextDialogue = (DialogueBase<T>)Selection.objects[i + 1];

                if (removeDirectTo)
                    dialogue.DirectTo.Disable();
                else
                {
                    dialogue.DirectTo.SetEnabled(true);

                    if (dialogue is Dialogue)
                        dialogue.DirectTo.SetNewDialogue(nextDialogue);
                    else if (dialogue is TrialDialogue)
                        dialogue.DirectTo.SetNewTrialDialogue(nextDialogue);
                }

                EditorUtility.SetDirty(dialogue);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ReplaceNames()
        {
            foreach (Object obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);

                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.RenameAsset(path, obj.name.Replace(replaceString, replaceWith));

                    EditorUtility.SetDirty(obj);
                }
                else if (obj is GameObject gameObject)
                {
                    gameObject.name = gameObject.name.Replace(replaceString, replaceWith);

                    EditorUtility.SetDirty(gameObject);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void NotAddToFront()
        {
            int currentNum = startIndex;

            foreach (Object obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);

                AssetDatabase.RenameAsset(path, baseName + currentNum);

                if (!decrease)
                    currentNum += increment;
                else
                    currentNum -= increment;

                EditorUtility.SetDirty(obj);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddToFront()
        {
            foreach (Object obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);

                AssetDatabase.RenameAsset(path, nameForFront + obj.name);

                EditorUtility.SetDirty(obj);
            }
        }
    }
}