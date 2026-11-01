//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueAssetManager : DialogueManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool registerOnEnable = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool destroyIfRegistrationExists = true;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            RegisterAsCurrentlyUsed();

            CheckDestroyOnExisting();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void RegisterAsCurrentlyUsed()
        {
            if (registerOnEnable && !ServiceLocator.HasService<IDialogueManager>())
                ServiceLocator.Register<IDialogueManager>(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckDestroyOnExisting()
        {
            if (destroyIfRegistrationExists && ServiceLocator.HasService(out IDialogueManager current) &&
                current != (this as IDialogueManager) && gameObject != null)
                Destroy(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnRegisterAsCurrentlyUsed();

        /// <summary>
        ///
        /// </summary>
        public virtual void UnRegisterAsCurrentlyUsed() => ServiceLocator.Unregister<IDialogueManager>(this);
    }
}