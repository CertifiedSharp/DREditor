// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ArgumentQuestionPanel
    {
        /// <summary>
        ///
        /// </summary>
        public Sprite panelShape;

        /// <summary>
        ///
        /// </summary>
        public Vector2 panelPos;

        /// <summary>
        ///
        /// </summary>
        public string questionText;

        /// <summary>
        ///
        /// </summary>
        public Vector2 symbolOffset = new(0, 0);

        /// <summary>
        ///
        /// </summary>
        public Vector2 symbolScale = new(1, 1);

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("flavourText")]
        public string flavorText;

        /// <summary>
        ///
        /// </summary>
        public int noOfLocks = 0;

        /// <summary>
        ///
        /// </summary>
        public Vector2 activeScale = new(1, 1);

        /// <summary>
        ///
        /// </summary>
        public Vector2 activePosition;

        /// <summary>
        ///
        /// </summary>
        public Vector2 stockScale = new(1, 1);

        /// <summary>
        ///
        /// </summary>
        public Vector2 stockPosition;


        /// <summary>
        ///
        /// </summary>
        /// <param name="panelShape"></param>
        public virtual void SetPanelShape(Sprite panelShape) => this.panelShape = panelShape;

        /// <summary>
        ///
        /// </summary>
        /// <param name="panelPos"></param>
        public virtual void SetPanelPos(Vector2 panelPos) => this.panelPos = panelPos;

        /// <summary>
        ///
        /// </summary>
        /// <param name="questionText"></param>
        public virtual void SetQuestionText(string questionText) => this.questionText = questionText;

        /// <summary>
        ///
        /// </summary>
        /// <param name="symbolOffset"></param>
        public virtual void SetSymbolOffset(Vector2 symbolOffset) => this.symbolOffset = symbolOffset;

        /// <summary>
        ///
        /// </summary>
        /// <param name="symbolScale"></param>
        public virtual void SetSymbolScale(Vector2 symbolScale) => this.symbolScale = symbolScale;

        /// <summary>
        ///
        /// </summary>
        /// <param name="flavorText"></param>
        public virtual void SetFlavorText(string flavorText) => this.flavorText = flavorText;

        /// <summary>
        ///
        /// </summary>
        /// <param name="noOfLocks"></param>
        public virtual void SetNoOfLocks(int noOfLocks) => this.noOfLocks = noOfLocks;

        /// <summary>
        ///
        /// </summary>
        /// <param name="activeScale"></param>
        public virtual void SetActiveScale(Vector2 activeScale) => this.activeScale = activeScale;

        /// <summary>
        ///
        /// </summary>
        /// <param name="activePosition"></param>
        public virtual void SetActivePosition(Vector2 activePosition) => this.activePosition = activePosition;

        /// <summary>
        ///
        /// </summary>
        /// <param name="stockScale"></param>
        public virtual void SetStockScale(Vector2 stockScale) => this.stockScale = stockScale;

        /// <summary>
        ///
        /// </summary>
        /// <param name="stockPosition"></param>
        public virtual void SetStockPosition(Vector2 stockPosition) => this.stockPosition = stockPosition;
    }
}