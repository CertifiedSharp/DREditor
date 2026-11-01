//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueAssetDataClearer : DialogueDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> SavingExtensionClearComponents = new();


        protected virtual void Awake()
        {
            var components = transform.root.GetComponentsInChildren<DialogueComponent>();

            foreach (var component in components)
                if (component is IClearDialogueSaveData && !SavingExtensionClearComponents.Contains(component))
                    SavingExtensionClearComponents.Add(component);
        }

        /// <inheritdoc/>
        public override void ClearSaveData()
        {
            foreach (IClearDialogueSaveData saver in SavingExtensionClearComponents.OfType<IClearDialogueSaveData>())
                saver?.ClearSaveData();
        }


        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public void Validate()
        {
            foreach(var component in SavingExtensionClearComponents)
                if (component is not IClearDialogueSaveData)
                    UnityEngine.Debug.Log(component.gameObject.name + " is not a valid Component! Please Remove!");
        }
    }
}