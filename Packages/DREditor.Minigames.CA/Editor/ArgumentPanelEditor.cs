// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.CA.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ArgumentPanelEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected Action callback;

        /// <summary>
        ///
        /// </summary>
        protected ArgumentQuestionPanel panel;

        /// <summary>
        ///
        /// </summary>
        protected ArgumentStockEditor stockDisplayer = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="callback"></param>
        public virtual void DisplayPanel(ArgumentQuestionPanel panel, Action callback)
        {
            this.callback = callback;

            this.panel = panel;

            DisplayPanelInfo();

            stockDisplayer.DisplayStock(panel, callback);
        }

        #region Display Panel Info

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPanelInfo()
        {
            EditorGUI.BeginChangeCheck();

            DisplayPanelShape();

            EditorGUILayout.LabelField("Comic Panel", EditorStyles.helpBox, GUILayout.Width(70));

            DisplayQuestionText();

            CAHandyFields.DisplayStockVectorField(
                new GUIContent(
                    "Position:",
                    "The panel's position relative to the comic page center, in world units."),
                panel.SetPanelPos,
                panel.panelPos);

            CAHandyFields.DisplayStockVectorField(
                new GUIContent(
                    "Symbol Scale: ",
                    "Scale of the question/check symbol displayed on the panel."),
                panel.SetSymbolScale,
                panel.symbolScale);

            CAHandyFields.DisplayStockVectorField(
                new GUIContent(
                    "Symbol Offset: ",
                    "Local position of the question/check symbol relative to the panel center."),
                panel.SetSymbolOffset,
                panel.symbolOffset);

            if (EditorGUI.EndChangeCheck())
                callback?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPanelShape()
        {
            GUILayout.Space(5);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(
                    new GUIContent(
                        "Sprite: ",
                        "Sprite used as the visual outline for comic panels, stock panels, and active panels."),
                    GUILayout.Width(130));

                panel.SetPanelShape(
                    (Sprite)EditorGUILayout.ObjectField(
                        panel.panelShape,
                        typeof(Sprite),
                        false,
                        GUILayout.Width(120)));
            }

            if (panel.panelShape != null)
                GUILayout.Label(
                    AssetPreview.GetAssetPreview(panel.panelShape),
                    GUILayout.MaxWidth(500),
                    GUILayout.MaxHeight(281.25f));

            GUILayout.Space(5);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayQuestionText()
        {
            EditorStyles.textField.wordWrap = true;

            GUILayout.Label(
                new GUIContent(
                    "Panel Question:",
                    "Question text displayed when this panel is highlighted."),
                GUILayout.Width(30));

            panel.SetQuestionText(
                EditorGUILayout.TextField(
                    panel.questionText,
                    GUILayout.Width(200),
                    GUILayout.Height(35)));
        }

        #endregion
    }
}