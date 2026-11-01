//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(TrialBuilder))]
    public class TrialBuilderEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected TrialBuilder builder;

        /// <summary>
        ///
        /// </summary>
        protected MinigameTypeDB MTDB;

        /// <summary>
        ///
        /// </summary>
        protected int index;

        /// <summary>
        ///
        /// </summary>
        protected TrialSequence sequence;


        protected virtual void Awake()
        {
            MTDB = (MinigameTypeDB)Resources.Load("Trial/MinigameTypeDB");

            if (MTDB == null)
                UnityEngine.Debug.LogError("MinigameTypeDatabase not found : Are you missing it in Resources/Trial/MinigameTypeDB ?");
        }

        protected virtual void OnEnable() => builder = target as TrialBuilder;

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            HandyFields.Label("Trial Builder");

            CreateForm();

            EditorUtility.SetDirty(builder);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            if (MTDB == null)
                return;

            ImportDiscussionsButton();

            DisplaySequences();

            DisplayAddSequenceButton();

            DisplayGateData();
        }

        #region Import Discussions Button

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportDiscussionsButton()
        {
            if (GUILayout.Button("Import Selected Discussions", GUILayout.Width(250)) &&
                EditorUtility.DisplayDialog("Import Trial Dialogue Discussions?",
                            "Import All?", "Yes", "No"))
            {
                foreach (var selectedObject in Selection.objects)
                    builder.sequences.Add(new TrialSequence((ScriptableObject)selectedObject));

                EditorUtility.SetDirty(builder);
            }
        }

        #endregion

        #region Display Sequences

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySequences()
        {
            if (builder.sequences.Count != 0)
            {
                for (int i = 0; i < builder.sequences.Count; i++)
                {
                    sequence = builder.sequences[i];

                    index = i;

                    using (new EditorGUILayout.HorizontalScope("Box"))
                        DisplaySequence();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySequence()
        {
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                GUILayout.Label(index.ToString(), GUILayout.Width(100));

                using (new EditorGUILayout.HorizontalScope())
                    DisplaySequenceInfo();

                DisplayMinigameAsset();
            }

            GUILayout.FlexibleSpace();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySequenceInfo()
        {
            GUILayout.Label("Minigame Type: ", GUILayout.Width(100));

            sequence.SetSequenceTypeNumber(EditorGUILayout.IntPopup(sequence.SequenceTypeNumber,
                MTDB.ToStringArray(), MTDB.ToIntegerArray(), GUILayout.Width(180)));

            sequence.SetMinigameType(MTDB.minigameTypes[sequence.SequenceTypeNumber]);

            sequence.SetSequenceDescription(HandyFields.StringArea("Description: ", sequence.SequenceDescription));

            DisplaySequencePosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayMinigameAsset()
        {
            using (new EditorGUILayout.HorizontalScope())
                sequence.SetMinigameAsset(HandyFields.UnityField(
                    new GUIContent("Minigame Asset: ", ""),
                    sequence.MinigameAsset, 7, 180, 20));
        }

        #region Display Sequence Position Buttons

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void DisplaySequencePosition()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(25)))
                    DisplaySequencePositionButtons();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySequencePositionButtons()
        {
            DisplayRemoveSequenceButton();

            DisplayMoveSequenceButtons();

            DisplayInsertSequenceButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRemoveSequenceButton()
        {
            if (builder.sequences.Count > 1)
            {
                if (GUILayout.Button(new GUIContent("-", "Delete this Line."), GUILayout.Width(20)) && builder.sequences.Count > 1)
                {
                    GUI.FocusControl(null);

                    builder.sequences.Remove(builder.sequences[index]);

                    serializedObject.ApplyModifiedProperties();

                    serializedObject.Update();

                    return;
                }
            }
            else
                GUILayout.Space(20f);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayMoveSequenceButtons()
        {
            DisplayMoveSequenceButton(false);

            DisplayMoveSequenceButton(true);

            GUILayout.Space(25f);

            GUILayout.FlexibleSpace();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="down"></param>
        protected virtual void DisplayMoveSequenceButton(bool down)
        {
            bool canDisplayButton = down ? index < builder.sequences.Count - 1 : index > 0;

            string displayButton = down ? "v" : "^";

            string displayButtonToolTip = down ? "Move this line down." : "Move this line up.";

            if (canDisplayButton &&
                GUILayout.Button(new GUIContent(displayButton, displayButtonToolTip), GUILayout.Width(20)))
            {
                GUI.FocusControl(null);

                if (down)
                    (builder.sequences[index], builder.sequences[index + 1]) = (builder.sequences[index + 1], builder.sequences[index]);
                else
                    (builder.sequences[index], builder.sequences[index - 1]) = (builder.sequences[index - 1], builder.sequences[index]);
            }
            else
                GUILayout.Space(20f);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayInsertSequenceButton()
        {
            if (GUILayout.Button(new GUIContent("+", "Add a new Line below."), GUILayout.Width(20)))
            {
                builder.sequences.Insert(index + 1, new TrialSequence());

                serializedObject.Update();
            }

            GUILayout.FlexibleSpace();
        }

        #endregion

        #endregion

        #region Display Add Sequence Button

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddSequenceButton()
        {
            if (GUILayout.Button("Add a new part of the trial!"))
                builder.sequences.Add(new TrialSequence());
        }

        #endregion

        #region Display Gate Data

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayGateData()
        {
            builder.SetEndSceneName(HandyFields.StringField("End Scene Name: ", builder.EndSceneName, 230, 130));

            builder.SetEndGate(HandyFields.UnityField(
                    new GUIContent("End Gate: ", ""),
                    builder.EndGate, 7, 120, 30));
        }

        #endregion
    }
}
#endif