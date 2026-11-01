//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class TimedDifficultyEditor : DifficultyEditorBase<List<TimerDifficulty>>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="timerDifficulties"></param>
        /// <returns></returns>
        public override List<TimerDifficulty> DisplaySettings(List<TimerDifficulty> timerDifficulties)
        {
            base.DisplaySettings(timerDifficulties);

            if (difficultyDatabase == null)
                return timerDifficulties;

            foldout = EditorGUILayout.Foldout(foldout, "Timers", largeFoldoutStyle);

            if (foldout)
            {
                GUILayout.Space(10);

                for (int i = 0; i < difficultyDatabase.States.Count; i++)
                {
                    if (i >= timerDifficulties.Count)
                        timerDifficulties.Add(new(i));

                    EditorGUILayout.LabelField("Time for: " + difficultyDatabase.GetStateName(timerDifficulties[i].difficulty));

                    using (new GUILayout.HorizontalScope())
                    {
                        timerDifficulties[i].SetMinutes(HandyFields.FloatField("Minutes: ", timerDifficulties[i].minutes, 100));

                        timerDifficulties[i].SetSeconds(HandyFields.FloatField("Seconds: ", timerDifficulties[i].seconds, 100));
                    }
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            return timerDifficulties;
        }
    }
}
#endif