//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Horizontal = UnityEditor.EditorGUILayout.HorizontalScope;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(BlackenedSelectBuilder))]
    public class BlackenedSelectBuilderEditor : TimedMinigameEditor<BlackenedSelectBuilder>
    {
        /// <summary>
        ///
        /// </summary>
        protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        protected TrialDialogue globalFuckUpDialogue = null;

        /// <summary>
        ///
        /// </summary>
        protected int[] characterIndexes;


        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CreateForm();

            EditorUtility.SetDirty(builder);

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            if (!Initialized())
                return;

            DisplayUtilityButtons();

            DisplayTimerSettings();

            DisplayQuestionText();

            DisplayGlobalFuckUpDialogue();

            DisplayBlackenedDatas();

            DisplayAddCharacterButton();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool Initialized()
        {
            if (!DatabaseUtility.InitializeDatabase(characterDatabase, out characterDatabase))
                return false;

            characterIndexes ??= characterDatabase.GetInts().ToArray();

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayUtilityButtons()
        {
            if (GUILayout.Button("Generate Base Characters", GUILayout.Width(200)) &&
                EditorUtility.DisplayDialog("Generate Base",
                "Are you sure you want to generate data? This will clear the current data!",
                "Yes", "No"))
                GenerateBuilderBase();

            if (GUILayout.Button("Clear Data", GUILayout.Width(200)) &&
                EditorUtility.DisplayDialog("Clear Data",
                "Are you sure you want to clear the current data?",
                "Yes", "No"))
                builder.selectDatas.Clear();

            GUILayout.Space(20);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void GenerateBuilderBase()
        {
            List<Character> characters = characterDatabase.GetFTECharacterList();

            builder.selectDatas.Clear();

            foreach (Character character in characters)
                builder.selectDatas.Add(new BlackenedSelectData(character.FirstName));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTimerSettings()
        {
            builder.SetDifficultyTimes(TimerDifficultyEditor.DisplaySettings(builder.times));

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayQuestionText()
        {
            builder.SetQuestionText(HandyFields.StringArea("Question: ", builder.questionText));

            GUILayout.Space(20);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayGlobalFuckUpDialogue()
        {
            using (new Horizontal())
            {
                EditorGUILayout.LabelField("Apply Mistake Dialogue: ", GUILayout.Width(150));

                if (GUILayout.Button("Apply") &&
                    EditorUtility.DisplayDialog("Apply Mistakes",
                    "Are you sure you want to apply this dialogue to all mistake fields?", "Yes", "No"))
                    foreach (var data in builder.selectDatas)
                        data.SetFuckUpDialogue(globalFuckUpDialogue);

                globalFuckUpDialogue = HandyFields.UnityField(globalFuckUpDialogue, 150, 20);
            }

            GUILayout.Space(40);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBlackenedDatas()
        {
            for (int i = 0; i < builder.selectDatas.Count; i++)
                if (!DisplayBlackenedData(i))
                    continue;

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool DisplayBlackenedData(int i)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new Horizontal())
                {
                    DisplayCharacterPopUp(i);

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        builder.selectDatas.RemoveAt(i);

                        return false;
                    }
                }

                DisplayAnswerField(i);

                DisplayFuckUpDialogue(i);
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayCharacterPopUp(int i)
        {
            BlackenedSelectData data = builder.selectDatas[i];

            int index = 0;

            if (characterDatabase.GetCharacter(data.characterName) is var character && character != null)
                index = characterDatabase.Characters.IndexOf(character);

            index = EditorGUILayout.IntPopup(index, characterDatabase.GetNames().ToArray(),
                characterIndexes, GUILayout.Width(110));

            data.SetCharacterName(characterDatabase.Characters[index].FirstName);

            GUILayout.Space(50);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayAnswerField(int i)
        {
            BlackenedSelectData data = builder.selectDatas[i];

            using (new Horizontal())
            {
                data.SetIsAnswer(HandyFields.Option(data.isAnswer, "Is Answer: "));

                GUILayout.Space(10);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayFuckUpDialogue(int i)
        {
            BlackenedSelectData data = builder.selectDatas[i];

            using (new Horizontal())
            {
                if (!data.isAnswer)
                {
                    EditorGUILayout.LabelField("Fuck Up Dialogue: ", GUILayout.Width(110));

                    data.SetFuckUpDialogue(HandyFields.UnityField(data.fuckUpDialogue, 150, 20));
                }
                else
                    data.SetFuckUpDialogue(null);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddCharacterButton()
        {
            if (GUILayout.Button("Add Character", GUILayout.Width(100)))
                builder.selectDatas.Add(new BlackenedSelectData());
        }
    }
}
#endif