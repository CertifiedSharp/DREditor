// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PanelInitializerBase) + ", " + nameof(ITotalStockListener) +
        "\n\n")]
    public class PanelInitializer : PanelInitializerBase, ITotalStockListener
    {
        /// <summary>
        ///
        /// </summary>
        protected List<int> unusedStocks;


        /// <inheritdoc/>
        public override void InitializePanel(ArgumentQuestionPanel panelData, Transform page)
        {
            base.InitializePanel(panelData, page);

            InitializePanelWithRandomStock();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePanelWithRandomStock()
        {
            QuestionPanelBase panelComp = qstnPanel.GetComponent<QuestionPanelBase>();

            panelComp.SetQuestion(panelData.questionText);

            int randomIndex = Random.Range(0, unusedStocks.Count);

            int stockIndex = unusedStocks[randomIndex];

            unusedStocks.RemoveAt(randomIndex);

            panelComp.SetStock(InitializeStock(panelData, stockIndex));
        }


        /// <inheritdoc/>
        public virtual void SetTotalStock(int totalStock) => unusedStocks = Enumerable.Range(0, totalStock).ToList();
    }
}