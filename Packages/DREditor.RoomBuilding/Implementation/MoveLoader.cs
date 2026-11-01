//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Talkables;
using System.Collections.Generic;
using System.Linq;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<MoveStateData, MoveData>) + 
        " for types <" + nameof(MoveStateData) + ", " + nameof(MoveData) + ">" +
        "\n\n")]
    public class MoveLoader : RoomStateHandlerBase<MoveStateData, MoveData>
    {
        /// <inheritdoc/>
        public override List<MoveData> Save() => SaveData();

        /// <inheritdoc/>
        public override void Unload(bool inEditor) { }

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<MoveData> baseDatas, List<MoveData> instanceDatas = null) 
            => Load(baseDatas, instanceDatas);

        /// <inheritdoc/>
        protected override bool LoadedInstanceValid(string dataInstance, out MoveData result)
            => base.LoadedInstanceValid(dataInstance, out result) && !string.IsNullOrEmpty(result.name);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<MoveData> SaveData()
        {
            List<MoveData> moveData = new();

            var moveList = FindObjectsByType<Moveable>();

            foreach (Moveable move in moveList)
                moveData.Add(move.Save());

            return moveData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadedData"></param>
        /// <param name="instanceData"></param>
        public static void Load(List<MoveData> loadedData, List<MoveData> instanceData = null)
            => Load(instanceData ?? loadedData);

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadedData"></param>
        public static void Load(List<MoveData> loadedData)
        {
            var moveList = FindObjectsByType<Moveable>();

            for (int i = 0; i < loadedData.Count; i++)
                for (int x = 0; x < moveList.Count(); x++)
                    if (moveList[x].gameObject.name == loadedData[i].name)
                        moveList[x].Load(loadedData[i]);
        }
    }
}