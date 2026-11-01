//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Talkables;
#if UNITY_EDITOR
using DREditor.Editor.Toolbox;
#endif
using System.Collections.Generic;
using System.Linq;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<ItemStateData, ItemData>) + 
        " for types <" + nameof(ItemStateData) + ", " + nameof(ItemData) + ">" +
        "\n\n")]
    public class ItemActorLoader : RoomStateHandlerBase<ItemStateData, ItemData>
    {
        /// <inheritdoc/>
        public override List<ItemData> Save() => SaveItems();
        
        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<ItemData> baseDatas, List<ItemData> instanceDatas = null) 
            => InitializeVisibleInstanceDatas(LoadItems(baseDatas, instanceDatas));

        /// <inheritdoc/>
        public override void Unload(bool inEditor) => UnloadItems();


        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="instanceData"></param>
        public static List<ItemData> LoadItems(List<ItemData> data, List<ItemData> instanceData = null)
        {
            ItemActor[] foundItems = (ItemActor[])FindObjectsByType(typeof(ItemActor));

            if (foundItems.Length == 0)
                UnityEngine.Debug.Log("REPORT: There are no items to be loaded in the scene!");

            for (int i = 0; i < foundItems.Length; i++)
            {
                for (int j = 0; j < data.Count; j++)
                {
                    if (foundItems[i].gameObject.name == data[j].iReference)
                    {
                        if (foundItems[i].isStaticItem)
                            continue;

                        var itemInstanceData = FindItemData(instanceData, data[j].iReference);

                        ApplyItemData(foundItems[i], (ItemData)data[j].Clone(), itemInstanceData);

                        break;
                    }
                }
            }

            return instanceData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceData"></param>
        /// <param name="itemReference"></param>
        /// <returns></returns>
        static ItemData FindItemData(List<ItemData> instanceData, string itemReference)
        {
            if (instanceData == null)
                return null;

            foreach (var instance in instanceData)
                if (instance.iReference == itemReference)
                    return instance;
            
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemData"></param>
        /// <param name="instanceData"></param>
        static void ApplyItemData(ItemActor item, ItemData itemData, ItemData instanceData = null)
        {
            item.conversations.Clear();

            foreach (var conversation in itemData.itemDialogues)
                item.conversations.Add(conversation);

            item.SetSelectability(itemData.iSelectable);

            if (instanceData != null)
                for (int i = 0; i < item.conversations.Count && i < instanceData.itemDialogues.Count; i++)
                    item.conversations[i].SetTriggered(instanceData.itemDialogues[i].triggered);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<ItemData> SaveItems()
        {
            List<ItemData> itemDatas = new();

            ItemActor[] items = (ItemActor[])FindObjectsByType(typeof(ItemActor));

            for (int i = 0; i < items.Length; i++)
                if (items[i].gameObject != null)
                    itemDatas.Add((ItemData)new ItemData(items[i].gameObject, items[i]).Clone());

            return itemDatas;
        }

        /// <summary>
        ///
        /// </summary>
        public static void UnloadItems()
        {
            var items = FindObjectsByType<ItemActor>();

            foreach (ItemActor item in items)
                item.ClearItemData();
        }

#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        [UnityEditor.MenuItem("DREditor/Utility/Scan/For Room State Datas with empty dialogues in items")]
        public static void FindRoomDatasWithNoDialogue()
        {
            var found = VerifyDialogues
                .FindAssetsByType<RoomStateBuilder>()
                .Where(builder => builder.Sections.Any(
                    section => section.Rooms.Any(
                        room => room.dataStates.Any(
                            stateData => stateData is ItemStateData data &&
                            data.datas.Any(itemData => itemData.itemDialogues.Contains(null)))
                        )
                    )
                )
                .ToList();

            foreach (var builder in found)
                UnityEngine.Debug.Log("Found: " + builder.name);

            if (found.Count == 0)
                UnityEngine.Debug.Log("Found None.");
        }

#endif
    }
}