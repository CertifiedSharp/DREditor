// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Closing Argument", fileName = "CA Builder")]
    public class ArgumentBuilder : MinigameBuilderBase
    {
        /// <summary>
        ///
        /// </summary>
        public List<ArgumentPage> pages = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <returns>
        ///
        /// </returns>
        public virtual (int, int) GetPageBounds(ArgumentPage page)
        {
            int stockCount = 0;

            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i].Equals(page))
                    return (stockCount, stockCount + pages[i].questionPanels.Count);

                stockCount += pages[i].questionPanels.Count;
            }

            return (-1, -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetTotalStock()
        {
            int stockCount = 0;

            for (int i = 0; i < pages.Count; i++)
                stockCount += pages[i].questionPanels.Count;

            return stockCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stock"></param>
        /// <returns>
        ///
        /// </returns>
        public virtual int GetStockIndex(ArgumentQuestionPanel stock)
        {
            int stockCounter = 0;

            for (int i = 0; i < pages.Count; i++)
            {
                int itemIndex = pages[i].questionPanels.IndexOf(stock);

                if (itemIndex != -1)
                    return stockCounter + itemIndex;
                else
                    stockCounter += pages[i].questionPanels.Count;
            }

            return -1;
        }
    }
}