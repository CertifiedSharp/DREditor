// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.CA.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ArgumentInitializerBase) +
        "\n\n")]
    public class ClosingArgumentInitializer : ArgumentInitializerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MonoBehaviour pageHandlerInitializer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject stockParent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cameraEnabled = null;


        [Header("")]
        [SerializeField] protected UnityEvent OnMinigameInitialized;


        /// <summary>
        ///
        /// </summary>
        protected int totalStock;

        /// <summary>
        ///
        /// </summary>
        protected IPagesInitializer pageInitializer = null;


        protected virtual void OnValidate()
        {
            if (pageHandlerInitializer != null && pageHandlerInitializer is not IPagesInitializer)
                UnityEngine.Debug.LogWarning($"ALERT: The assigned Page Handler does not inherit {nameof(IPagesInitializer)}!");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePageInitializer()
        {
            if (pageHandlerInitializer != null && pageHandlerInitializer is not IPagesInitializer)
                UnityEngine.Debug.LogError($"ALERT: The assigned Page Handler does not inherit {nameof(IPagesInitializer)}!");
            else
                pageInitializer ??= pageHandlerInitializer as IPagesInitializer;
        }

        /// <inheritdoc/>
        public override void InitializeArgument(ScriptableObject builder)
        {
            Cursor.lockState = CursorLockMode.Locked;

            SetTotalStock(((ArgumentBuilder)builder).GetTotalStock());

            InitializeStockListeners();

            RelinkStocks(false);

            InitializePageInitializer();

            pageInitializer.InitializePages(((ArgumentBuilder)builder).pages);

            if (!cameraEnabled.Value)
                cameraEnabled.SetValue(true);

            OnMinigameInitialized?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="totalStock"></param>
        protected virtual void SetTotalStock(int totalStock) => this.totalStock = totalStock;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeStockListeners()
        {
            var parent = transform.parent != null ? transform.parent : transform;

            foreach (var totalStockListener in parent.GetComponentsInChildren<ITotalStockListener>())
                totalStockListener.SetTotalStock(totalStock);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inPreview"></param>
        protected virtual void RelinkStocks(bool inPreview)
        {
            ClosingArgumentStockBase[] stocks = stockParent.GetComponentsInChildren<ClosingArgumentStockBase>();

            for (int i = totalStock; i < stocks.Length; i++)
            {
                stocks[i].Rewire();

                if (inPreview)
                    stocks[i].gameObject.SetActive(false);
                else
                    Destroy(stocks[i].gameObject);
            }
        }


        protected virtual void OnDestroy() => cameraEnabled.SetValue(false);
    }
}