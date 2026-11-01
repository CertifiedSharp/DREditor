//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GenerateDialogueWizardBase : GenerateWizardBase
    {
        [Tooltip("")]
        [SerializeField] protected int startingNumber = 1;

        [Tooltip("")]
        [TextArea(5, 10)]
        [SerializeField] protected string textArea = "";

        [Tooltip("")]
        [SelectType]
        [SerializeReference] protected IDialogueImporter dialogueImporter = new DialogueImporter();

        /// <summary>
        /// 
        /// </summary>
        protected float progress = 0;


        protected GenerateDialogueWizardBase() : base(true) => assetPrefix = "Dialogues";

        protected override bool OnEnableWizardValid() => ImportValid();

        protected override bool ImportValid() => dialogueImporter != null && dialogueImporter.ImporterValid();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        protected virtual void Import<T, T2>() where T : DialogueCore where T2 : Line, new()
        {
            List<T> dialoguesToSave = new();
            
            if (textArea != "")
            {
                Debug.Log("Dialogue Importer: Using Text area for Importing");

                dialogueImporter.Import<T, T2>(dialoguesToSave, new StringReader(textArea), textArea.Length);
            }
            else
            {
                Debug.Log("Dialogue Importer: Using Text File for Importing");

                if (!TryGetFilePath(out string path) && EditorUtility.DisplayDialog("Selecting Text File Failed.",
                "ALERT: No input file selected. Process aborted", "OK"))
                    return;

                dialogueImporter.Import<T, T2>(dialoguesToSave, new StreamReader(path, Encoding.UTF8), new FileInfo(path).Length);
            }

            SaveDialogueObjects(dialoguesToSave);
        }

        protected virtual void SaveDialogueObjects<T>(List<T> dialoguesToSave) where T : DialogueCore
        {
            string folder = FolderUtility.CreateIntermediateFolders(targetDirectory);

            int dialogueNumber = startingNumber;

            AssetDatabase.StartAssetEditing();

            for (int i = 0; i < dialoguesToSave.Count; i++)
            {
                T dia = dialoguesToSave[i];

                dia.SetName(dialogueNumber + "");

                string diaPath = folder + "/" + assetPrefix + "_" + dialogueNumber + "_.asset";

                AssetDatabase.CreateAsset(dia, diaPath);

                dialogueNumber++;

                float saveProgress = (i + 1) / dialoguesToSave.Count;

                progress += saveProgress * 0.1f;

                EditorUtility.DisplayProgressBar("Processing...", "Saving dialogues to disk.", progress);
            }

            AssetDatabase.StopAssetEditing();

            AssetDatabase.SaveAssets();

            EditorUtility.ClearProgressBar();

        }

        protected virtual bool TryGetFilePath(out string path)
        {
            path = EditorUtility.OpenFilePanelWithFilters("Open dialogue text file", "",
                    new string[] { "Text file", "txt" });

            return path.Length != 0;
        }
    }
}