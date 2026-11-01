// Original Author: KHeartz
// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Items;
using DREditor.Processables;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Presents/Present", fileName = "Present")]
    public class Present : AttributableInventoryItem<IPresentData>, IIdentifiable
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int id;
        

        public virtual int ID { get => id; set => id = value; }
    }
}