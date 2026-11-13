//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Services;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [CustomDialogueEvent("MapEntryChange", typeof(MapEntryChange))]
    [ClassHeader(nameof(DialogueEventHandler) + 
        "\n\n")]
    public class MapEntryChanger : DialogueEventHandler
    {
        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (o is MapEntryChange change && change != null)
                ChangeMapEntry(change);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="change"></param>
        protected virtual void ChangeMapEntry(MapEntryChange change)
        {
            if (change.unlockArea != "")
                ServiceLocator.GetService<IMapManager>().AddUnlockedArea(change.unlockArea);

            if (change.Entry.AreaEntries.Count == 0)
                return;

            change.Entry.SetUnlockedAreas(ServiceLocator.GetService<IMapManager>().GetInstanceData().UnlockedAreas);

            ServiceLocator.GetService<IMapManager>().SetInstanceData(change.Entry);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Reset() => SetDefaultEventString("MapEntryChange");
    }
}