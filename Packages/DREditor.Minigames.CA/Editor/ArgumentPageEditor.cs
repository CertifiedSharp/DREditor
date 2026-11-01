// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.CA.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ArgumentPageEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected ArgumentPanelEditor questionPanelDisplayer = new();

        /// <summary>
        ///
        /// </summary>
        protected Action callback;

        /// <summary>
        ///
        /// </summary>
        protected int stockCounter;

        /// <summary>
        ///
        /// </summary>
        protected int currentStock = 1;

        /// <summary>
        ///
        /// </summary>
        protected int totalStock = 0;

        /// <summary>
        ///
        /// </summary>
        protected ArgumentBuilder builder;

        /// <summary>
        ///
        /// </summary>
        protected ArgumentPage page;

        /// <summary>
        ///
        /// </summary>
        protected int pageIndex;


        /// <summary>
        ///
        /// </summary>
        public virtual void Reset() => stockCounter = 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="totalStock"></param>
        public virtual void Initialize(int totalStock) => this.totalStock = totalStock;

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentStock"></param>
        public virtual void SetCurrentStock(int currentStock) => this.currentStock = currentStock;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetCurrentStockIndex() => currentStock;


        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="page"></param>
        /// <param name="pageIndex"></param>
        /// <param name="callback"></param>
        public virtual void DisplayPage(ArgumentBuilder builder, ArgumentPage page, int pageIndex, Action callback)
        {
            this.builder = builder;

            this.callback = callback;

            this.page = page;

            this.pageIndex = pageIndex;

            DisplayPageInfo();
        }

        #region Show Page Info

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPageInfo()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                DisplayPageSprite();

                DisplayQuestionPanels();

                DisplayPageEndButtons();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPageSprite()
        {
            GUILayout.Space(5);

            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(
                        new GUIContent(
                            "Page Sprite:",
                            "Sprite used as the visual for this argument page in the Closing Argument builder."),
                        GUILayout.Width(120));

                    using (new EditorGUILayout.VerticalScope())
                    {
                        EditorGUI.BeginChangeCheck();

                        page.SetPageSprite(
                            (Sprite)EditorGUILayout.ObjectField(page.pageSprite, typeof(Sprite), false, GUILayout.Width(120)));

                        if (EditorGUI.EndChangeCheck())
                            callback?.Invoke();
                    }
                }

                if (page.pageSprite != null)
                    GUILayout.Label(
                        AssetPreview.GetAssetPreview(page.pageSprite),
                        GUILayout.MaxWidth(500),
                        GUILayout.MaxHeight(281.25f));
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayQuestionPanels()
        {
            if (page.questionPanels.Count > 0)
            {
                GUILayout.Space(20);

                for (int j = 0; j < page.questionPanels.Count; j++)
                    DisplayQuestionPanel(j);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="j"></param>
        protected virtual void DisplayQuestionPanel(int j)
        {
            using (new EditorGUILayout.VerticalScope())
            {
                stockCounter++;

                EditorGUILayout.LabelField($"  Stock {stockCounter}", EditorStyles.boldLabel);

                using (new EditorGUILayout.HorizontalScope("Box"))
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        questionPanelDisplayer.DisplayPanel(page.questionPanels[j], callback);

                        GUILayout.Space(7.5f);

                        // Max width set so that flexible space works correctly and the 'Remove Stock' button is always visible
                        using (new EditorGUILayout.HorizontalScope(GUILayout.MaxWidth(Screen.width - 50)))
                        {
                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button(new GUIContent("Remove Stock", "Remove the current stock panel."),
                                GUILayout.Width(100)))
                            {
                                // get index of removed stock
                                int stockIndex = builder.GetStockIndex(page.questionPanels[j]);

                                page.questionPanels.RemoveAt(j);

                                callback?.Invoke();

                                if (currentStock != 1 && stockIndex < currentStock)
                                    currentStock--;
                            }
                        }

                        GUILayout.Space(2.5f);
                    }
                }

                GUILayout.Space(10);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPageEndButtons()
        {
            // Max width set so that flexible space works correctly and the 'Remove Page' button is always visible
            using (new EditorGUILayout.HorizontalScope(GUILayout.MaxWidth(Screen.width - 42)))
            {
                if (page.questionPanels.Count >= 0 && totalStock < 10)
                {
                    if (GUILayout.Button(new GUIContent("Add Stock", "Add a new question panel to this page."),
                        GUILayout.Width(100)))
                    {
                        page.AddNewPanel();

                        callback?.Invoke();
                    }
                }

                GUILayout.FlexibleSpace();
            }
        }

        #endregion

        #region Display Preview

        /// <summary>
        ///
        /// </summary>
        /// <param name="previewCallback"></param>
        public virtual void DisplayPreviewStockButtons(Action previewCallback)
        {
            string previewLabel = totalStock == 0 ?
                "No Stock to Preview" :
                $"Previewing stock {currentStock}/{totalStock}";

            GUILayout.Label(previewLabel);

            GUI.enabled = !(currentStock <= 1);

            StockButtonPreview("<", -1, previewCallback);

            GUI.enabled = !(currentStock >= totalStock);

            StockButtonPreview(">", 1, previewCallback);

            GUI.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="addStock"></param>
        /// <param name="previewCallback"></param>
        protected virtual void StockButtonPreview(string buttonText, int addStock, Action previewCallback)
        {
            if (GUILayout.Button(buttonText))
            {
                currentStock += addStock;

                previewCallback?.Invoke();
            }
        }

        #endregion
    }
}