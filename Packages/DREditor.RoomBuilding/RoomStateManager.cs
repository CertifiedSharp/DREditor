//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using DREditor.Progressing.Handling;
using DREditor.RoomStateBuilding;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public class RoomStateManager : RoomManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        public RoomStateBuilder Builder = null;

        /// <summary>
        ///
        /// </summary>
        public List<MonoBehaviour> loadHandlers = new();

        /// <summary>
        ///
        /// </summary>
        public List<IRoomStateLoader> loaders = new();

        /// <summary>
        ///
        /// </summary>
        public List<IRoomStateSaver> savers = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="Builder"></param>
        public virtual void SetBuilder(RoomStateBuilder Builder) => this.Builder = Builder;

        /// <inheritdoc/>
        protected override void Awake()
        {
            dontDestroyOnLoad = false;

            base.Awake();

            InitializeHandlers();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void InitializeHandlers()
        {
            savers = loadHandlers.OfType<IRoomStateSaver>().ToList();

            loaders = loadHandlers.OfType<IRoomStateLoader>().ToList();
        }

        /// <inheritdoc/>
        public override bool HasBase(int chapterNum, object pValue)
        {
            bool result = Builder.Sections[chapterNum].HasDataFor((Objective)pValue);

            UnityEngine.Debug.Log("Has Base for current room: " + result);

            return result;
        }

        /// <inheritdoc/>
        public override object SaveRoomState(object pValue)
        {
            InstanceListWrapper<InstanceListWrapper<string>> instanceTemp = new();

            foreach (var saver in savers)
                instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>((List<string>)saver.SaveInstance()));

            foreach (var listWrapper in instanceTemp.wrappedInstances)
                if (listWrapper.wrappedInstances != null)
                    foreach (var wrappedList in listWrapper.wrappedInstances)
                        UnityEngine.Debug.Log(wrappedList);

            UnityEngine.Debug.Log(JsonUtility.ToJson(instanceTemp));

            return JsonUtility.ToJson(instanceTemp);
        }

        /// <inheritdoc/>
        public override bool BaseIsPersistent(object pValue)
        {
            var result = GetBaseData((Objective)pValue);

            return result != null && result.isPersistent;
        }

        /// <inheritdoc/>
        public override void LoadRoomState(object pValue, object data)
        {
            var currentChapter = ServiceLocator.GetService<ProgressionManagerBase>().GetCurrentChapter();

            RoomStateSection current = Builder.Sections
                .Find(section => section.Chapter == currentChapter);

            if (current == null || !currentChapter.HasObjective((Objective)pValue))
            {
                UnityEngine.Debug.LogWarning("REPORT: ROOM BUILDER DID NOT HAVE A ROOM SECTION FOR THAT CHAPTER");

                current = Builder.Sections[0];
            }

            RoomStateData baseRoom = GetBaseData((Objective)pValue, current);

            baseRoom = baseRoom == null ? OnBaseRoomEmpty((Objective)pValue, current) : baseRoom;

            if (data != null && baseRoom != null)
                LoadInstance(baseRoom, data);
            else
                LoadRoomFromHandlers(baseRoom);

            onRoomInstanceLoaded.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        protected virtual RoomStateData OnBaseRoomEmpty(Objective pValue, RoomStateSection current)
        {
            UnityEngine.Debug.Log("Couldn't find Base Room data for Objective: " + pValue.Description + " " +
                "within chapter: " + current.Chapter.name + ", Checking for Room Shell.");

            int shellIndex = Builder.Sections.IndexOf(current);

            if (shellIndex < Builder.Shells.Count)
            {
                UnityEngine.Debug.Log("Shell Data has been found! Loading Shell...");

                return Builder.Shells[shellIndex];
            }

            UnityEngine.Debug.Log($"No Shell Data Has been Filled at index {shellIndex}, Loading Empty Shell");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        protected virtual RoomStateData GetBaseData(Objective pValue)
        {
            RoomStateSection current = Builder.Sections
                .Find(section => section.Chapter == ServiceLocator.GetService<ProgressionManagerBase>().GetCurrentChapter());

            return GetBaseData(pValue, current);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        protected virtual RoomStateData GetBaseData(Objective pValue, RoomStateSection current) =>
            current.Rooms.FirstOrDefault(room => room.progressionValue.Description == pValue.Description);

        /// <summary>
        ///
        /// </summary>
        /// <param name="room"></param>
        public virtual void LoadRoomFromHandlers(RoomStateData room)
        {
            if (room != null)
                foreach (var loader in loaders)
                    loader?.LoadData(room.dataStates);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="room"></param>
        /// <param name="instanceRoom"></param>
        protected virtual void LoadInstance(RoomStateData room, object instanceRoom = null)
        {
            foreach (var loader in loaders)
                loader.LoadInstance(room.dataStates, instanceRoom);
        }

        /// <inheritdoc/>
        public override void UnloadAll(bool inEditor)
        {
            foreach (var loader in loaders)
                loader.Unload(inEditor);
        }
    }
}