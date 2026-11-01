//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Investigations;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.TBS
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(TBSelectBuilder))]
    public class TBSelectBuilderEditor : TimedMinigameEditor<TBSelectBuilder>
    {
        /// <summary>
        ///
        /// </summary>
        protected EvidenceDatabase evidenceDatabase;

        /// <summary>
        ///
        /// </summary>
        protected TrialDialogue allDialogue;


        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!DatabaseUtility.InitializeDatabase(evidenceDatabase, out evidenceDatabase))
                return;

            CreateForm();

            EditorUtility.SetDirty(builder);

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            SetBasicData();

            if (!EvaluateChapterEvidence())
                return;

            SetGlobalWrongDialogue();

            DisplayTruthBullets();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetBasicData()
        {
            builder.SetDamage(HandyFields.IntField("Damage on Wrong: ", builder.damageOnWrong, 25, 7));

            builder.SetDifficultyTimes(TimerDifficultyEditor.DisplaySettings(builder.times));

            builder.SetQuestion(HandyFields.StringArea("Question: ", builder.question));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool EvaluateChapterEvidence()
        {
            builder.SetChapter(HandyFields.IntField("Chapter: ", builder.chapter, 25, 7));

            if (builder.chapter < 0 || builder.chapter > evidenceDatabase.Evidences.Count - 1)
            {
                GUILayout.Label("Chapter has no Evidence", GUILayout.Width(220));

                return false;
            }

            if (builder.selections.Count == 0)
                for (int i = 0; i < evidenceDatabase.Evidences[builder.chapter].TruthBullets.Count; i++)
                    builder.selections.Add(new TBSelection());

            if (evidenceDatabase.Evidences[builder.chapter].TruthBullets.Count != builder.selections.Count)
            {
                int diff = builder.selections.Count - evidenceDatabase.Evidences[builder.chapter].TruthBullets.Count;

                if (diff > 0) // Removed TB from Evidence
                    builder.selections.RemoveAt(builder.selections.Count - diff);
                else // Added TB to Evidence
                {
                    diff *= -1;

                    for (int i = 0; i < diff; i++)
                        builder.selections.Add(new TBSelection());
                }
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetGlobalWrongDialogue()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Copy All Wrong Dialogue", GUILayout.Width(170));

                allDialogue = HandyFields.UnityField(allDialogue, 170, 25);

                if (GUILayout.Button("Paste To All"))
                    for (int i = 0; i < builder.selections.Count; i++)
                        builder.selections[i].SetWrongDialogue(allDialogue);

                if (GUILayout.Button("Clear All"))
                    for (int i = 0; i < builder.selections.Count; i++)
                        builder.selections[i].SetWrongDialogue(null);
            }

            using (new GUILayout.HorizontalScope())
                GUILayout.Label("", GUILayout.Width(120));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTruthBullets()
        {
            Evidence evidence = evidenceDatabase.Evidences[builder.chapter];

            for (int i = 0; i < builder.selections.Count; i++)
            {
                builder.selections[i].SetBullet(evidence.TruthBullets[i]);

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(builder.selections[i].bulletOption.DisplayName, GUILayout.Width(120));

                    builder.selections[i].SetAnswer(EditorGUILayout.Toggle(builder.selections[i].isAnswer));
                }

                if (!builder.selections[i].isAnswer)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("On Wrong Dialogue: ", GUILayout.Width(120));

                        builder.selections[i].SetWrongDialogue(HandyFields.UnityField(builder.selections[i].wrongDialogue, 170, 25));
                    }
                }
                else
                    builder.selections[i].SetWrongDialogue(null);

                using (new GUILayout.HorizontalScope())
                    GUILayout.Label("", GUILayout.Width(120));
            }
        }
    }
}
#endif