//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class InspectorGoToLineDebugger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference load = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => SubscribeToDebugEvent();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToDebugEvent() 
            => load.action.started += OnDebugCurrentAssetLine;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnsubscribeToDebugEvent();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToDebugEvent() 
            => load.action.started -= OnDebugCurrentAssetLine;


        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void OnDebugCurrentAssetLine(CallbackContext ctx)
        {
    #if UNITY_EDITOR
            UnityEngine.Debug.Log($"{ToString()}: " +
                $"Attempting to display the dialogue asset currently being read to the inspector window.");
    #endif

            DialogueCore dia = ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentDialogue();

            if (dia != null)
                GoToAsset(dia);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void GoToAsset(Object obj)
        {
    #if UNITY_EDITOR

            UnityEditor.Selection.activeObject = obj;

            LogAssetLine(obj.name);
    #else
            GUIUtility.systemCopyBuffer = LogAssetLine(obj.name);
    #endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        protected virtual string LogAssetLine(string assetName)
        {
            int currentLineNumber = ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentLineNum();

            string logText = "At Dialogue: " + assetName + " at line: " + currentLineNumber;

            UnityEngine.Debug.Log(logText);

            return logText;
        }
    }
}