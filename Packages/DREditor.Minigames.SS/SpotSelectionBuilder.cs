//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Spot Selection", fileName = "SS Builder")]
    public class SpotSelectionBuilder : MinigameBuilderBase
    {
        /// <summary>
        ///
        /// </summary>
        public Sprite sprite = null;

        /// <summary>
        ///
        /// </summary>
        public List<Spot> spots = new();

        /// <summary>
        ///
        /// </summary>
        public string question = "";

        /// <summary>
        ///
        /// </summary>
        public int damageOnWrong = 1;


        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        public virtual void SetSprite(Sprite sprite) => this.sprite = sprite;

        /// <summary>
        ///
        /// </summary>
        /// <param name="question"></param>
        public virtual void SetQuestion(string question) => this.question = question;

        /// <summary>
        ///
        /// </summary>
        /// <param name="damageOnWrong"></param>
        public virtual void SetDamageOnWrong(int damageOnWrong) => this.damageOnWrong = damageOnWrong;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Spot GetSpotWithName(string name) => spots.Where(spot => spot.spotName == name).FirstOrDefault();


#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        public string testCanvas = "SS_Canvas";

        /// <summary>
        ///
        /// </summary>
        public string testCG = "SS_CG";


        /// <summary>
        ///
        /// </summary>
        /// <param name="testCanvas"></param>
        public virtual void SetTestCanvas(string testCanvas) => this.testCanvas = testCanvas;

        /// <summary>
        ///
        /// </summary>
        /// <param name="testCG"></param>
        public virtual void SetTestCG(string testCG) => this.testCG = testCG;

#endif
    }
}