//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(SpotSelectionBuilder))]
    public class SpotSelectionBuilderEditor : TimedMinigameEditor<SpotSelectionBuilder>
    {
        /// <summary>
        ///
        /// </summary>
        protected SpotSelectionToolEditor toolEditor;

        /// <summary>
        ///
        /// </summary>
        protected SpotsEditor spotsEditor;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (builder != null && toolEditor == null)
                toolEditor = new(builder);

            if (builder != null && spotsEditor == null)
                spotsEditor = new(builder);
        }


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
            DisplayBaseInformation();

            DisplayBuildTools();

            DisplayBuilder();

            DisplaySpots();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBaseInformation()
        {
            builder.SetQuestion(HandyFields.StringArea("Question: ", builder.question));

            builder.SetDamageOnWrong(HandyFields.IntField("Damage on Wrong: ", builder.damageOnWrong, 25, 7));

            builder.SetDifficultyTimes(TimerDifficultyEditor.DisplaySettings(builder.times));
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBuildTools() => toolEditor?.Display();


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBuilder()
        {
            DisplaySelectionImage();

            DisplayDeleteSpotsButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySelectionImage()
        {
            builder.SetSprite(HandyFields.SpriteField("Room Image: ", builder.sprite));

            GUILayout.Space(30);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDeleteSpotsButton()
        {
            if (builder.spots.Count > 0)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(60);

                    if (GUILayout.Button("Delete All Spots", GUILayout.Width(120)) &&
                        EditorUtility.DisplayDialog(
                            "Clear All",
                            "Are you sure you want to Delete ALL spots from the list?",
                            "Yes", "No"))
                        builder.spots.Clear();
                }
            }

            GUILayout.Space(30);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpots() => spotsEditor?.Display();
    }
}
#endif