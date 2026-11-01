//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class RestoreControlsUIPostDialogue : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string scanningKey = "trialdialogue";

        /// <summary>
        ///
        /// </summary>
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected string originalKey = "";


        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager dialogueManager;


        protected virtual void OnEnable() => currentControlsKey.AddListener(CheckInitializeKey);

        protected virtual void OnDisable()
        {
            currentControlsKey.RemoveListener(CheckInitializeKey);

            dialogueManager?.GetFinisher().UnsubscribeToOnFinishOfDialogue(RestoreKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        protected virtual void CheckInitializeKey(string key)
        {
            if (IsScannedKey(key))
                OnScannedKeyFound();
            else
                InitializeOriginalKey();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual bool IsScannedKey(string key) => key == scanningKey;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnScannedKeyFound()
        {
            currentControlsKey.RemoveListener(CheckInitializeKey);

            if (ServiceLocator.HasService(out dialogueManager))
                dialogueManager.GetFinisher().SubscribeToOnFinishOfDialogue(RestoreKey);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeOriginalKey()
        {
            if (currentControlsKey.Value != scanningKey)
                originalKey = currentControlsKey.Value;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RestoreKey()
        {
            dialogueManager?.GetFinisher().UnsubscribeToOnFinishOfDialogue(RestoreKey);

            currentControlsKey.SetValue(originalKey);

            currentControlsKey.AddListener(CheckInitializeKey);
        }
    }
}