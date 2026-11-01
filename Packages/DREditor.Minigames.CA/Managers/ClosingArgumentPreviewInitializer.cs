// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ClosingArgumentInitializer) + ", " + nameof(ArgumentInitializerBase) + ", " + 
        nameof(IArgumentPreviewInitializer) +
        "\n\n")]
    public class ClosingArgumentPreviewInitializer : ClosingArgumentInitializer, IArgumentPreviewInitializer
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ArgumentBuilder builder;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int previewCurrentPage;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int previewStockIndex;


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Initialize Preview")]
        public void Call() => InitializePreview(builder, previewCurrentPage, previewStockIndex);


        public virtual void InitializePreview(ScriptableObject builder, int currentPage, int stockIndex)
        {
            SetTotalStock(((ArgumentBuilder)builder).GetTotalStock());

            InitializePageInitializer();

            pageInitializer.InitializePages(((ArgumentBuilder)builder).pages);

            RelinkStocks(true);

            InitializeActiveStockPanel(stockIndex);

            InvokePagePreviewers(currentPage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stockIndex"></param>
        protected virtual void InitializeActiveStockPanel(int stockIndex)
        {
            IActiveStockPanel activeStockPanel = transform.parent.GetComponentInChildren<IActiveStockPanel>();

            activeStockPanel.InitializeActivePanel(stockIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPage"></param>
        protected virtual void InvokePagePreviewers(int currentPage)
        {
            var pagePreviewers = transform.parent.GetComponentsInChildren<IClosingArgumentPagePreviewer>();

            foreach (var previewer in pagePreviewers)
                previewer.InitializePreviewPages(currentPage);
        }
    }
}