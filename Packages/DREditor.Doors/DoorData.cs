//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class DoorData
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public string doorName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool identified;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string displayName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string unidentifiedDisplayName = "???";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public ScriptableObject roomLoadOptions;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public LockData lockData;


        /// <summary>
        ///
        /// </summary>
        /// <param name="doorName"></param>
        public virtual void SetName(string doorName) => this.doorName = doorName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="displayName"></param>
        public virtual void SetDisplayName(string displayName) => this.displayName = displayName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="identified"></param>
        public virtual void SetIdentified(bool identified) => this.identified = identified;


        /// <summary>
        ///
        /// </summary>
        public virtual void ClearLockData() => lockData.ClearData();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            DoorData copy = (DoorData)MemberwiseClone();

            DoorData data = new()
            {
                doorName = copy.doorName,
                displayName = copy.displayName,
                unidentifiedDisplayName = copy.unidentifiedDisplayName,
                roomLoadOptions = copy.roomLoadOptions,
                lockData = (LockData)lockData.Clone(),
                identified = copy.identified
            };

            return data;
        }
    }
}