//Author: Benjamin "Sweden" Jillson : sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public class MultiPanelEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly TrialLineEditor<TrialLine> lineEditor = new(false);


        /// <summary>
        ///
        /// </summary>
        protected List<MultiPanel> lines;

        /// <summary>
        ///
        /// </summary>
        protected List<TrialLine> convertedLines;

        /// <summary>
        ///
        /// </summary>
        protected DialogueCore dialogue;

        /// <summary>
        ///
        /// </summary>
        protected bool linesDirty = true;


        /// <summary>
        ///
        /// </summary>
        private const int IntFieldWidth = 50;

        /// <summary>
        ///
        /// </summary>
        private const int GUISpacing = 5;


        /// <summary>
        ///
        /// </summary>
        /// <param name="foldout"></param>
        /// <param name="lines"></param>
        /// <param name="dialogue"></param>
        /// <param name="foldOutText"></param>
        /// <returns></returns>
        public virtual bool EditMultiPanels(bool foldout, List<MultiPanel> lines, DialogueCore dialogue, string foldOutText)
        {
            if (this.lines != lines || this.dialogue != dialogue)
            {
                this.lines = lines;

                this.dialogue = dialogue;

                SetLinesDirty(true);
            }

            GUILayout.Space(GUISpacing);

            foldout = HandyFields.Foldout(foldout, foldOutText, DisplayMultiPanels, EditorStyles.boldLabel);

            EditorGUILayout.EndFoldoutHeaderGroup();

            return foldout;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayMultiPanels()
        {
            if (lines == null)
                return;

            GUI.color = Color.white;

            DisplayAddButton();

            using (new GUILayout.VerticalScope("Box", GUILayout.Width(170)))
            {
                if (linesDirty)
                    RebuildConvertedLines();

                DisplayConvertedPanels();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddButton()
        {
            if (HandyFields.Button("Add Panel"))
            {
                lines.Add(new());

                SetLinesDirty(true);
            }
        }

        protected virtual void SetLinesDirty(bool linesDirty) => this.linesDirty = linesDirty;

        /// <summary>
        ///
        /// </summary>
        protected virtual void RebuildConvertedLines()
        {
            convertedLines = new();

            foreach (var panel in lines)
                if (panel != null)
                    convertedLines.Add(panel.ConvertToLine());

            if (dialogue != null)
                lineEditor.Initialize(dia: dialogue, lines: convertedLines);

            SetLinesDirty(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayConvertedPanels()
        {
            for (int i = 0; i < convertedLines.Count && i < lines.Count; i++)
            {
                DisplayMultiPanel(i);

                if (DisplayRemoveButton(i))
                    return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayMultiPanel(int i)
        {
            if (lines == null)
                return;

            lines[i].SetPanelNum(HandyFields.IntField("Panel Number: ", lines[i].panelNum, IntFieldWidth));

            lineEditor.EditLine(i);

            lines[i].Merge(convertedLines[i]);

            GUILayout.Space(GUISpacing);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool DisplayRemoveButton(int i)
        {
            bool result = HandyFields.Button("Remove Panel");

            if (lines != null && result)
            {
                lines.Remove(lines[i]);

                convertedLines.Remove(convertedLines[i]);

                SetLinesDirty(true);
            }

            return result;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static class MultiPanelEditorUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="panelNumbers"></param>
        /// <param name="characterNumbers"></param>
        /// <param name="dialogue"></param>
        public static void PanelCharacters(ref List<int> panelNumbers, ref List<int> characterNumbers, DialogueCore dialogue)
        {
            int intFieldWidth = 50;

            float guiSpacing = 10;

            if (HandyFields.Button("Add Panel Character"))
            {
                panelNumbers.Add(panelNumbers.Count);

                characterNumbers.Add(-1);
            }

            using (new GUILayout.VerticalScope("Box", GUILayout.Width(170)))
            {
                for (int i = 0; i < panelNumbers.Count && i < characterNumbers.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        panelNumbers[i] = HandyFields.IntField("", panelNumbers[i], intFieldWidth);

                        EditorGUILayout.LabelField(": ", GUILayout.MaxWidth(intFieldWidth));

                        var prependedArray = ContainerUtil.PrependedList(dialogue.Speakers.GetNames().ToArray(), "<No Character>");

                        characterNumbers[i] = EditorGUILayout.IntPopup(characterNumbers[i], prependedArray,
                            ContainerUtil.Iota(prependedArray.Length, -1),
                            GUILayout.Width(130));

                        GUILayout.Space(guiSpacing);

                        if (HandyFields.Button("x"))
                        {
                            panelNumbers.RemoveAt(i);

                            characterNumbers.RemoveAt(i);

                            return;
                        }
                    }

                    GUILayout.Space(guiSpacing);
                }
            }

            GUILayout.Space(guiSpacing);
        }
    }
}