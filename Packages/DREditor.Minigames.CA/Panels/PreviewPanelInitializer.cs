// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PanelInitializerBase) + ", " + nameof(ITotalStockListener) +
        "\n\n")]
    public class PreviewPanelInitializer : PanelInitializerBase, ITotalStockListener
    {
        /// <summary>
        ///
        /// </summary>
        protected int stockIndex = 0;


        /// <inheritdoc/>
        public override void InitializePanel(ArgumentQuestionPanel panelData, Transform page)
        {
            base.InitializePanel(panelData, page);

            InitializeQuestionPanelData();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeQuestionPanelData()
        {
            QuestionPanelBase panelComp = qstnPanel.GetComponent<QuestionPanelBase>();

            panelComp.SetQuestion(panelData.questionText);

            panelComp.SetStock(InitializeStock(panelData, stockIndex));

            panelComp.OnPreview();

            stockIndex++;
        }


        /// <inheritdoc/>
        protected override void InitializeQuestionPanelVisual()
        {
            base.InitializeQuestionPanelVisual();

            // Animators don't work in previews and cause performance issues
            if (qstnPanel.TryGetComponent(out Animator animator))
                animator.enabled = false;
        }


        /// <inheritdoc/>
        public virtual void SetTotalStock(int totalStock) => stockIndex = 0;
    }
}