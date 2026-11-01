// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.CA.Editor
{
    // Basically, I can't have more than one editor target the same type, which doesn't make any sense.
    //[CustomEditor(typeof(ArgumentBuilder))]

    /// <summary>
    ///
    /// </summary>
    public class ArgumentBuilderEditor<T> : TimedMinigameEditor<T> where T : ArgumentBuilder
    {
        /// <summary>
        ///
        /// </summary>
        protected int currentPage = 1;

        /// <summary>
        ///
        /// </summary>
        protected Action previewCallback;

        /// <summary>
        ///
        /// </summary>
        protected ArgumentPageEditor argumentPageDisplayer = new();


        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Closing Argument Builder", EditorStyles.boldLabel);

            CreateForm();

            EditorUtility.SetDirty(builder);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            argumentPageDisplayer.Reset();

            builder.SetDifficultyTimes(TimerDifficultyEditor.DisplaySettings(builder.times));

            using (new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(Screen.width)))
            {
                DisplayPages();

                DisplayAddPageButton();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPages()
        {
            if (builder.pages.Count != 0)
                for (int i = 0; i < builder.pages.Count; i++)
                    DisplayPage(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        protected virtual void DisplayPage(int index)
        {
            ArgumentPage page = builder.pages[index];

            EditorGUILayout.LabelField($"Page {index + 1}", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope("Box"))
                argumentPageDisplayer.DisplayPage(builder, page, index + 1, previewCallback);

            DisplayRemovePageButton(page, index + 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageNumber"></param>
        protected virtual void DisplayRemovePageButton(ArgumentPage page, int pageNumber)
        {
            if (GUILayout.Button("Remove Page", GUILayout.Width(100)))
            {
                var currentStock = argumentPageDisplayer.GetCurrentStockIndex();

                if (currentPage != 1 && (pageNumber < currentPage || currentPage == builder.pages.Count))
                    currentPage -= 1;

                if (currentStock != 1)
                {
                    (int first, int last) = builder.GetPageBounds(page);

                    if (currentStock > last)
                        currentStock -= page.questionPanels.Count;
                    else if (currentStock >= first)
                        currentStock = first - 1;
                }

                builder.pages.Remove(page);

                if (currentStock <= 0)
                    currentStock = 1;

                argumentPageDisplayer.SetCurrentStock(currentStock);

                previewCallback?.Invoke();
            }

            GUILayout.Space(4);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddPageButton()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5);

                if (GUILayout.Button(new GUIContent("Add Page", "Add a new page."), GUILayout.Width(100)))
                {
                    builder.pages.Add(new ArgumentPage());

                    previewCallback?.Invoke();
                }
            }
        }
    }
}