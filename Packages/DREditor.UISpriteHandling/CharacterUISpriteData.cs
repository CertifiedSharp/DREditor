//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UISpriteHandling
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Characters/Protag UI Sprite Data", fileName = "UI Sprite Data")]
    public class CharacterUISpriteData : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        public string charName = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public float x = 0;

        /// <summary>
        ///
        /// </summary>
        public float y = 0;


        /// <summary>
        ///
        /// </summary>
        /// <param name="charName"></param>
        public virtual void SetCharacterName(string charName) => this.charName = charName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void SetSpritePosition(float x, float y)
        {
            this.x = x;

            this.y = y;
        }
    }
}