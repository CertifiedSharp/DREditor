//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Services;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Dialogues/Trial Discussion", fileName = "TrialDiscussion")]
    public class TrialDiscussion : ScriptableObject, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public TrialDialogue[] trialDialogues;


        /// <summary>
        ///
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        public virtual bool CheckIfSaveDialogue()
        {
            foreach (TrialDialogue currentTrialDialogue in trialDialogues)
                if (ServiceLocator.GetService<IDialogueManager>().GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<TrialLine>(currentTrialDialogue))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns>
        ///
        /// </returns>
        public virtual int GetIndexOfTrialDialogue(TrialDialogue dialogue)
        {
            for (int i = 0; i < trialDialogues.Length; i++)
                if (dialogue.name == trialDialogues[i].name)
                    return i;

            UnityEngine.Debug.LogWarning(
                $"TrialDiscussion: WARNING: None of the Trial Dialogues in asset {name} matched the name of the " +
                $"searched dialogue {dialogue.name}. Returning Index 0.");

            return 0;
        }


#if UNITY_EDITOR

        protected virtual void OnValidate() => EditorUtility.SetDirty(this);

#endif
    }
}