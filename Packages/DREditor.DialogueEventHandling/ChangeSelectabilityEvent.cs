//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class ChangeSelectabilityEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => SetDefaultEvent();


        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o = null) => ChangeSelectability(o);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void ChangeSelectability(object o = null)
        {
            ChangeItemSelect data = (ChangeItemSelect)o;

            var selectable = FindObjectsByType<SelectableObject>().Where(n => n.gameObject.name == data.objectName).FirstOrDefault();

            if (selectable != null)
                selectable.SetSelectability(data.isSelectable);
            else
                UnityEngine.Debug.LogError($"{ToString()}: Could not find item object in the scene!");
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ChangeItemSelect");
    }
}