//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Flairs.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class GenerateFlairDebugDialogue : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeReference, SelectType]
        [SerializeField] protected IGenerateFlairDialogueSetting activeSetting;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeReference, SelectType]
        [SerializeField] protected List<IFlairEvent> events = new();


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Flair/Generate Flair Debug Dialogue")]
        public static void GenerateFlairDialogueWizard() => DisplayWizard<GenerateFlairDebugDialogue>("Flair Dialogue Wizard", "Create");


        protected virtual void OnWizardCreate()
        {
            AssetDatabase.StartAssetEditing();
            
            try
            {
                activeSetting?.ExecuteOption(events);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();

                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif