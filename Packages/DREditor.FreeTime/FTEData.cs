//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/FTE/FTE Data", fileName = "FTEData")]
    public class FTEData : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string characterName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int level;


        public virtual void SetData(string name, int level = 0)
        {
            this.name = name;

            characterName = name;

            this.level = level;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        public virtual void AddLevel(int level = 1) => this.level += level;

        /// <summary>
        ///
        /// </summary>
        public virtual void Validate()
        {
            if (string.IsNullOrEmpty(name))
                name = characterName;
            else if (string.IsNullOrEmpty(characterName))
                characterName = name;
        }
    }
}