//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PlaceEntry : MapBaseData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool enableFastTravelForThisEntryInstance = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<MarkerVessel> MarkerVessels = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<MarkerData> MarkerDatas = new();


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Save()
        {
#if UNITY_EDITOR
            if (!MarkerVessels.Contains(null))
                MarkerDatas = GetMarkerData();
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<MarkerData> GetMarkerData()
        {
            List<MarkerData> data = new();

            foreach (MarkerVessel v in MarkerVessels)
                if (v != null)
                    data.Add(v.markerData);
                else
                    UnityEngine.Debug.LogWarning("ALERT: MARKER VESSEL WAS NULL!");

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="markerName"></param>
        /// <returns></returns>
        public virtual PlaceEntry Contains(string markerName)
        {
            for (int i = 0; i < MarkerDatas.Count; i++)
                if (MarkerDatas[i].name == markerName)
                    return this;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableFastTravelForThisEntryInstance"></param>
        public virtual void SetFastTravel(bool enableFastTravelForThisEntryInstance) 
            => this.enableFastTravelForThisEntryInstance = enableFastTravelForThisEntryInstance;
    }
}