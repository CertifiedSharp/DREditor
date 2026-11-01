//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class ClearMapAreaData : DialogueEventHandler
    {
        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o = null) 
            => ServiceLocator.GetService<IMapManager>().GetInstanceData().AreaEntries.Clear();


        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ClearMapUI");
    }
}