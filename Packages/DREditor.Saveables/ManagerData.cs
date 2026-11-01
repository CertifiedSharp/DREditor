//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace DREditor.Saveables
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ManagerData
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("DataList")]
        public List<ManagerDataSet> dataSets = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool HasDuplicate()
        {
            foreach (var item in dataSets)
                foreach (var info in dataSets)
                    if (item != info && SetsMatch(item, info))
                        return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="setOne"></param>
        /// <param name="setTwo"></param>
        /// <returns></returns>
        protected virtual bool SetsMatch(ManagerDataSet setOne, ManagerDataSet setTwo)
        {
            bool result = setOne.subID == setTwo.subID && setOne.typeName == setTwo.typeName;

            UnityEngine.Debug.LogWarning("2 Managers of type : " + setOne.GetType().Name +
                            " . Contain the same SubIDs! This is not allowed because otherwise the " +
                            "ISaveManager will not load Save Data Correctly!");

            return result;
        }
    }
}