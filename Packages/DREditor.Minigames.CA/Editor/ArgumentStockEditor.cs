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
    public class ArgumentStockEditor
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
        /// <param name="panel"></param>
        /// <param name="callback"></param>
        public virtual void DisplayStock(ArgumentQuestionPanel panel, Action callback)
        {
            this.callback = callback;

            this.panel = panel;

            DisplayStockPanel();

            DisplayStockVectors();
        }

        #region Display Stock Panel

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStockPanel()
        {
            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Stock Panel", EditorStyles.helpBox, GUILayout.Width(67));

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(
                    new GUIContent(
                        "Number of Locks:",
                        "How many preceding panels must be solved before this one unlocks (0–9)."),
                    GUILayout.Width(110));

                GUI.SetNextControlName("locksInput");

                panel.SetNoOfLocks(EditorGUILayout.IntField(panel.noOfLocks, GUILayout.Width(33)));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (panel.noOfLocks < 0 || panel.noOfLocks >= 10)
                    panel.SetNoOfLocks(0);

                callback?.Invoke();
            }
        }

        #endregion

        #region Display Stock Vectors

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStockVectors()
        {
            EditorGUI.BeginChangeCheck();

            DisplayFlavorText();

            GUILayout.Space(5);

            DisplayStockScale();

            DisplayStockPosition();

            GUILayout.Space(5);

            if (EditorGUI.EndChangeCheck())
                callback?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFlavorText()
        {
            GUILayout.Label(
                new GUIContent(
                    "Stock Text:",
                    "Narrative or descriptive line shown when this stock panel is hovered."),
                GUILayout.Width(70));

            panel.SetFlavorText(
                EditorGUILayout.TextField(
                    panel.flavorText,
                    GUILayout.Width(200),
                    GUILayout.Height(35)));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStockScale()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                CAHandyFields.DisplayStockVectorField(
                    new GUIContent(
                        "Sprite Scale: ",
                        "World-space scale of the stock's display sprite."),
                    panel.SetStockScale,
                    panel.stockScale);

                CAHandyFields.DisplayStockVectorField(
                    new GUIContent(
                        "Active Sprite Scale: ",
                        "Scale of the sprite when shown in the active panel."),
                    panel.SetActiveScale,
                    panel.activeScale);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStockPosition()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                CAHandyFields.DisplayStockVectorField(
                    new GUIContent(
                        "Sprite Offset: ",
                        "Local sprite position relative to the stock panel center."),
                    panel.SetStockPosition,
                    panel.stockPosition);

                CAHandyFields.DisplayStockVectorField(
                    new GUIContent(
                        "Active Sprite Offset: ",
                        "Local sprite position relative to the active panel center."),
                    panel.SetActivePosition,
                    panel.activePosition);
            }
        }

        #endregion
    }
}