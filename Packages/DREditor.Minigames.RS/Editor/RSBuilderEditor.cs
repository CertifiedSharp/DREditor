//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Minigames.Debates.Editor;
using DREditor.Minigames.RS.Managers;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.RS.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(RSBuilder))]
    public class RSBuilderEditor : RSEditorBase<RSGroup>
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly MultiPanelEditor introPanelEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly MultiPanelLineEditor<RSPanel> responsePanelEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly string lineFieldName = "Lines";

        /// <summary>
        ///
        /// </summary>
        protected readonly string panelsFieldName = "Panels";

        /// <summary>
        ///
        /// </summary>
        protected readonly string responsePanelsFieldName = "ResponsePanels";

        /// <summary>
        ///
        /// </summary>
        protected readonly Color orangeColor = new(1.0f, 0.64f, 0.0f);


        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty propList;

        /// <summary>
        ///
        /// </summary>
        protected bool showRebuttalSpecifics = false;

        /// <summary>
        ///
        /// </summary>
        protected bool showSection = true;

        /// <summary>
        ///
        /// </summary>
        protected bool showResponsePanels = false;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            SetDialogue(target as RSBuilder);

            propList = serializedObject.FindProperty(lineFieldName);
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            lineEditor.Initialize(dia, serializedObject, this,
                propList.GetArrayElementAtIndex(CurrentSection).FindPropertyRelative(panelsFieldName));

            CreateForm();

            EditorUtility.SetDirty(builder);

            serializedObject.ApplyModifiedProperties();
        }

        /// <inheritdoc/>
        protected override void DisplayDebateSpecifics()
        {
            base.DisplayDebateSpecifics();

            using (new EditorGUILayout.VerticalScope("Box"))
                showRebuttalSpecifics = HandyFields.Foldout(showRebuttalSpecifics, "Rebuttal Specifics:",
                    ShowRebuttalSpecifics, EditorStyles.boldLabel);

            HandySplitters.UISplitter(orangeColor);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowRebuttalSpecifics()
        {
            DisplayRebuttalFields();

            AutoImportMultiPanels();

            AutoAssignPanels();

            MultiPanelEditorUtility.PanelCharacters(ref builder.assignedPanelNumbers, ref builder.panelCharacterNumbers, builder);

            AdditionalPanelsButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRebuttalFields()
        {
            GUILayout.Space(guiSpacing);

            builder.InterjectObject = HandyFields.UnityField(new GUIContent("Interject Object:"), builder.InterjectObject);

            GUILayout.Space(guiSpacing);

            builder.StartingRSDialogue = HandyFields.UnityField(new GUIContent("Starting MultiDialogue:"),
                builder.StartingRSDialogue);

            GUILayout.Space(guiSpacing);

            builder.DefaultRetreatDialogue = HandyFields.UnityField(new GUIContent("Default Retreat Dialogue:"),
                (MultiTrialDialogue)builder.DefaultRetreatDialogue);

            GUILayout.Space(guiSpacing);

            builder.ShowIntroPanels = introPanelEditor.EditMultiPanels(builder.ShowIntroPanels,
                builder.introPanels, builder, "Intro Panel Movements");

            GUILayout.Space(guiSpacing);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AutoImportMultiPanels()
        {
            if (builder != null &&
                HandyFields.Button("Add Imported CharPanels") &&
                EditorUtility.DisplayDialog("Convert", "Are you sure you want to Apply Multiline Panels?" +
                    "\n THIS WILL ADD ON TOP OF ALL PANELS AND CANNOT BE UNDONE!", "Yes", "No"))
            {
                List<int> foundChars = new();

                foreach (var section in builder.Lines)
                    foreach (var panel in section.Panels)
                        builder.ImportSpeakers(foundChars, panel, panel);

                foreach (var section in builder.Lines)
                    section.ImportMultiPanels(foundChars);
            }

            GUILayout.Space(guiSpacing);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AutoAssignPanels()
        {
            builder.AutoAssignPanelNums = HandyFields.BoolField(builder.AutoAssignPanelNums, "Auto Assign Panel Nums: ");

            for (int i = 0; i < builder.Lines.Count; i++)
            {
                if (builder.AutoAssignPanelNums && builder.Lines[i] != null)
                {
                    builder.AssignPanelNums(builder.Lines[i].Panels);

                    builder.AssignPanelNums(builder.Lines[i].ResponsePanels);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AdditionalPanelsButton()
        {
            if (HandyFields.Button("Apply Additional Panels"))
                AddMultiPanels();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddMultiPanels()
        {
            foreach (var section in builder.Lines)
                foreach (var panel in section.Panels)
                    foreach (int characterNumber in builder.panelCharacterNumbers)
                        if (AddMultiPanel(panel, characterNumber))
                            continue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="characterNumber"></param>
        /// <returns></returns>
        protected virtual bool AddMultiPanel(RSPanel panel, int characterNumber)
        {
            var multiPanel = new MultiPanel
            {
                CamAnimIdx = panel.CamAnimIdx
            };

            multiPanel.SetSpeakerNum(characterNumber);

            panel.multiPanels.Add(multiPanel);

            return characterNumber == panel.SpeakerNumber;
        }


        /// <inheritdoc/>
        protected override void EditPanels()
        {
            GUILayout.Space(guiSpacing);

            if (ValidSection())
                DisplaySectionHeader();

            DisplaySectionData();

            DisplaySection();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidSection()
            => builder.Lines != null && builder.Lines.Count > 0 && CurrentSection < builder.Lines.Count && CurrentSection >= 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySectionHeader()
        {
            using (new EditorGUILayout.HorizontalScope("Box"))
            {
                if (CurrentSection == 0)
                    GUI.enabled = false;

                if (HandyFields.Button("Previous Section"))
                    CurrentSection -= 1;

                if (CurrentSection == 0)
                    GUI.enabled = true;

                EditorGUILayout.LabelField($"Section: {CurrentSection + 1} / {builder.Lines.Count}");

                RemoveSectionButton();

                GUILayout.Space(guiSpacing);

                if (CurrentSection != builder.Lines.Count - 1 && HandyFields.Button("Next Section"))
                    CurrentSection += 1;

                builder.SectionIndex = CurrentSection;
            }

            GUILayout.Space(guiSpacing);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveSectionButton()
        {
            if (HandyFields.Button("Remove Section") &&
                    EditorUtility.DisplayDialog("Remove Section", "Are you sure you want to remove this section?" +
                    "\nIt cannot be undone!", "Yes", "No"))
            {
                builder.Lines.RemoveAt(CurrentSection);

                if (CurrentSection >= builder.Lines.Count && builder.Lines.Count > 0)
                    CurrentSection = builder.Lines.Count - 1;
                else if (builder.Lines.Count == 0)
                    CurrentSection = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySectionData()
        {
            if (!ValidSection())
                return;

            RSGroup section = builder.Lines[CurrentSection];

            using (new EditorGUILayout.VerticalScope("Box"))
            {
                EditorGUILayout.LabelField(new GUIContent("Section Data:"), EditorStyles.boldLabel);

                GUILayout.Space(guiSpacing);

                DisplayConvertField(section.Panels);

                GUILayout.Space(guiSpacing);

                if (ValidSection())
                    section.CutCount = HandyFields.IntField("Cuts:", section.CutCount);

                GUILayout.Space(guiSpacing);

                section.EndGroupDialogue = HandyFields.UnityField(new GUIContent("End Panel Dialogue:"),
                    section.EndGroupDialogue);

                GUILayout.Space(guiSpacing);

                section.RetreatDialogue = HandyFields.UnityField(new GUIContent("Specific Retreat Dialogue:"),
                    section.RetreatDialogue);
            }

            GUILayout.Space(guiSpacing);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySection()
        {
            using (new EditorGUILayout.VerticalScope("Box"))
                showSection = HandyFields.Foldout(showSection, "RS Section:", base.EditPanels, EditorStyles.boldLabel);
        }


        /// <inheritdoc/>
        protected override void EditFooter()
        {
            using (new EditorGUILayout.VerticalScope("IN Title"))
                base.EditFooter();

            HandySplitters.UISplitter(Color.cyan);

            using (new EditorGUILayout.VerticalScope("Box"))
                if (builder.Lines != null && builder.Lines.Count > 0 && CurrentSection < builder.Lines.Count && CurrentSection >= 0)
                    showResponsePanels = HandyFields.Foldout(showResponsePanels, "Response Panels:",
                        DisplayResponseSection, EditorStyles.boldLabel);

            using (new EditorGUILayout.VerticalScope("IN Title"))
                if (HandyFields.Button("New Response Panel"))
                    builder.Lines[CurrentSection].ResponsePanels.Add(new());

            responsePanelEditor.DrawHoverSprite();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayResponseSection()
        {
            if (EditPanelMode)
                return;

            SerializedProperty propLines = propList
                .GetArrayElementAtIndex(CurrentSection)
                .FindPropertyRelative(responsePanelsFieldName);

            responsePanelEditor.Initialize(dia, serializedObject, this, propLines, builder.Lines[CurrentSection].ResponsePanels);

            EditResponsePanel = false;

            DisplayConvertField(builder.Lines[CurrentSection].ResponsePanels);

            for (int i = 0; i < builder.Lines[CurrentSection].ResponsePanels.Count; i++)
                responsePanelEditor.EditLine(i);

            EditResponsePanel = EditPanelMode;

            GUILayout.Space(guiSpacing);
        }
    }
}