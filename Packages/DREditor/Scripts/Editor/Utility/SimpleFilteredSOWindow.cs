//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleFilteredSOWindow<T> : FilteredSOPickerWindow<T>
    {
        /// <summary>
        ///
        /// </summary>
        protected Rect currentlyDrawnRow;


        /// <summary>
        ///
        /// </summary>
        protected readonly float rowSpacing = 28;

        /// <summary>
        ///
        /// </summary>
        protected readonly float rowHeight = 24;

        /// <summary>
        ///
        /// </summary>
        protected readonly float iconSpacing = 3;

        /// <summary>
        ///
        /// </summary>
        protected readonly float iconWidth = 18;

        /// <summary>
        ///
        /// </summary>
        protected readonly float iconHeight = 18;

        /// <summary>
        ///
        /// </summary>
        protected readonly string refreshButtonText = "Refresh";


        protected virtual void OnGUI()
        {
            DrawToolbar();

            DrawCountLabel();

            DrawNoneRow();

            HandySplitters.UISplitter(Color.gray);

            DrawFilteredAssetList();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                EditorGUI.BeginChangeCheck();

                search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField, GUILayout.ExpandWidth(true));

                if (EditorGUI.EndChangeCheck())
                    ApplySearch();

                if (HandyFields.ToolbarButton(refreshButtonText))
                    RefreshAssets();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawCountLabel()
            => EditorGUILayout.LabelField($"{GetAssetCountString()} implementing {typeof(T).Name}",
                EditorStyles.centeredGreyMiniLabel);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetAssetCountString() => $"{filteredObjects.Count} asset{(filteredObjects.Count != 1 ? "s" : "")}";

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawNoneRow() => DrawRow(null);

        protected override void DrawRow(ScriptableObject asset)
        {
            currentlyDrawnRow = EditorGUILayout.GetControlRect(false, rowHeight);

            DrawRowBackground(asset);

            DrawRowIcon(asset);

            DrawRowLabel(asset);

            HandyEditor.PingAndDoOnClick(asset, currentlyDrawnRow, Assign);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void DrawRowBackground(ScriptableObject asset)
        {
            if (asset == current)
                EditorGUI.DrawRect(currentlyDrawnRow, selectedBackgroundColor);
            else if (currentlyDrawnRow.Contains(Event.current.mousePosition))
                EditorGUI.DrawRect(currentlyDrawnRow, hoverBackgroundColor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void DrawRowIcon(ScriptableObject asset)
        {
            if (asset != null)
            {
                Texture icon = EditorGUIUtility.ObjectContent(asset, asset.GetType()).image;

                if (icon != null)
                    GUI.DrawTexture(
                        new Rect(currentlyDrawnRow.x + iconSpacing, currentlyDrawnRow.y + iconSpacing, iconWidth, iconHeight),
                        icon,
                        ScaleMode.ScaleToFit);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void DrawRowLabel(ScriptableObject asset)
        {
            string display = asset == null ? "None"
                : $"{asset.name}   <color=#{ColorUtility.ToHtmlStringRGBA(Color.gray)}><i>{asset.GetType().Name}</i></color>";

            GUIStyle labelStyle = new(EditorStyles.label)
            {
                richText = true,
                alignment = TextAnchor.MiddleLeft
            };

            if (asset == current)
                labelStyle.fontStyle = FontStyle.Bold;

            Rect labelRect = new(currentlyDrawnRow.x + rowSpacing, currentlyDrawnRow.y,
                currentlyDrawnRow.width - rowSpacing, currentlyDrawnRow.height);

            GUI.Label(labelRect, display, labelStyle);
        }
    }
}
#endif