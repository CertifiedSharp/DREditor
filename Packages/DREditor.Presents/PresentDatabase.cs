//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Presents/Database", fileName = "PresentDatabase")]
    public class PresentDatabase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        public List<Present> presents = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Present GetPresentByID(int id) => presents.Where(p => p.ID == id).FirstOrDefault();
    }
}