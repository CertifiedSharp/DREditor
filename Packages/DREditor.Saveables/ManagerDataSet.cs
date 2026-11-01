//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Saveables
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ManagerDataSet
    {
        /// <summary>
        ///
        /// </summary>
        public int subID;

        /// <summary>
        ///
        /// </summary>
        public string data;

        /// <summary>
        ///
        /// </summary>
        public string typeName;

        public ManagerDataSet(ISaveManager manager)
        {
            if (manager != null)
            {
                subID = manager.SubID;

                data = manager.Save();

                typeName = manager.GetType().Name;
            }
            else
                UnityEngine.Debug.Log($"REPORT: Logging notice of manager data set without interface." +
                    $"\nSPECULATION: It is likely an old save file is present.");
        }
    }
}