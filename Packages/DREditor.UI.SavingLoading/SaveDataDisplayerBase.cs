//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SaveDataDisplayerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        /// <param name="id"></param>
        public abstract void Display(Dictionary<string, object> saveDatas, int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public abstract void OnSaveNotFound(int id);
    }
}