//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.SubAreas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<SubAreaStateData, SubAreaData>) + 
        " for types <" + nameof(SubAreaStateData) + ", " + nameof(SubAreaData) + ">" +
        "\n\n")]
    public class SubAreaLoader : RoomStateHandlerBase<SubAreaStateData, SubAreaData>
    {
        /// <inheritdoc/>
        public override List<SubAreaData> Save() => SaveData();

        /// <inheritdoc/>
        public override void Unload(bool inEditor) { }

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<SubAreaData> baseDatas, List<SubAreaData> instanceDatas = null) 
            => Load(baseDatas, instanceDatas);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<SubAreaData> SaveData()
        {
            List<SubAreaData> areaData = new();

            var areaList = FindObjectsByType<MonoBehaviour>().OfType<ISubArea>().ToList();

            foreach (ISubArea manager in areaList)
                areaData.Add((SubAreaData)manager.Save());

            return areaData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseData"></param>
        /// <param name="loadedData"></param>
        public static void Load(List<SubAreaData> baseData, List<SubAreaData> loadedData = null)
        {
            ReferseInstanceData(baseData, loadedData);

            ApplySubAreaData(baseData, loadedData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseData"></param>
        /// <param name="loadedData"></param>
        static void ReferseInstanceData(List<SubAreaData> baseData, List<SubAreaData> loadedData = null)
        {
            if (loadedData != null && loadedData.Count > 0)
            {
                for (int i = 0; i < baseData.Count; i++)
                {
                    try
                    {
                        SubAreaData subBaseData = baseData[i];

                        SubAreaData subInstanceData = loadedData[i];

                        if (subBaseData.codeName != subInstanceData.codeName)
                        {
                            UnityEngine.Debug.Log("CALLED REVERSE FOR SUBAREAS");

                            loadedData.Reverse();

                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogWarning("ALERT: A BASE OR INSTANCE PIECE OF SUB AREA DATA COULDN'T BE FOUND \n" +
                            ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseData"></param>
        /// <param name="loadedData"></param>
        static void ApplySubAreaData(List<SubAreaData> baseData, List<SubAreaData> loadedData = null)
        {
            var areaList = UnityEngine.Object.FindObjectsByType<MonoBehaviour>().OfType<ISubArea>().ToList();

            for (int i = 0; i < baseData.Count; i++)
            {
                var subAreaData = loadedData != null && i < loadedData.Count ?
                    SubAreaData.MergeInstance(baseData[i], loadedData[i]) : baseData[i];

                areaList[i].Load(subAreaData);
            }
        }
    }
}