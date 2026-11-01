//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DebugDialogueBaseLine<T> : MonoBehaviour where T : Line
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LineReader lineReader;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => DialogueCore.OnDebugLine += DebugLine;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueCore.OnDebugLine -= DebugLine;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="line"></param>
        protected virtual void DebugLine(DialogueCore dialogue, Line line)
        {
            if (dialogue != null && dialogue is DialogueBase<T> dialogueType && line is T lineType)
            {
                UnityEngine.Debug.Log("Testing Line Gameplay for: " + dialogueType.GetType().Name + ": " + dialogue.name);

                StartCoroutine(lineReader.PlayDialogueLine(dialogueType, line));
            }
        }
    }
}