//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.Gates;
using DREditor.Progressing;
using DREditor.RoomBuilding;
using DREditor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.DebugTesting.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(DebugTester))]
    public class DebugTesterEditor : DebugTesterBaseEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected DebugTester tester;

        /// <summary>
        ///
        /// </summary>
        protected GateDatabase gateDatabase;

        /// <summary>
        ///
        /// </summary>
        protected ProgressionDatabase progressionDatabase;

        /// <summary>
        ///
        /// </summary>
        protected List<RoomStateBuilder> RoomStateBuilders;

        /// <summary>
        ///
        /// </summary>
        protected UnityEditor.Editor cachedEditor;


        protected virtual void OnEnable()
        {
            tester = target as DebugTester;

            gateDatabase = DatabaseUtility.LoadDatabase<GateDatabase>();

            progressionDatabase = DatabaseUtility.LoadDatabase<ProgressionDatabase>();
        }

        protected virtual void OnDisable()
        {
            if (cachedEditor != null)
                DestroyImmediate(cachedEditor);
        }


        /// <inheritdoc/>
        protected override bool PassInitialization() => gateDatabase != null && progressionDatabase != null;

        /// <summary>
        ///
        /// </summary>
        protected override void CreateForm()
        {
            base.CreateForm();

            DisplayPauseData();

            DisplayState();

            DisplayChapter();

            DisplayGates();

            DisplayPointInTime();
        }

        #region Display Objects and Events

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPauseData()
        {
            if (tester.pauseData != null)
                DrawFoldoutInspector(tester.pauseData, ref cachedEditor);

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayState()
        {
            string[] names = tester.stateDatabase != null ? tester.stateDatabase.GetStateNames() : new string[0];

            int dropdownWidth = names.Length > 0 ? names[tester.state].Length : 0;

            tester.SetState(HandyFields.IntDropdown("State: ", tester.state, names, dropdownWidth));

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayChapter()
        {
            string[] names = progressionDatabase != null ? progressionDatabase.GetAllChapterNames() : new string[0];

            int dropdownWidth = names.Length > 0 ? names[tester.chapterIndex].Length : 0;

            tester.SetChapterIndex(HandyFields.IntDropdown("Chapter: ", tester.chapterIndex, names, dropdownWidth));

            GUILayout.Space(10);
        }

        #endregion

        #region Display Gates

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayGates()
        {
            DisplayGoingToGate();

            using (new EditorGUILayout.HorizontalScope())
            {
                DisplayInvestigationGates();

                DisplayFromGate();
            }

            DisplayDefaultFromGate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayGoingToGate()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Going To: ", GUILayout.MaxWidth(100));

                var gateNames = ContainerUtil.PrependedList(gateDatabase.GetGateNames(), "No Gate");

                int[] nameIndexes = ContainerUtil.Iota(gateNames.Length, -1);

                int gateIndex = EditorGUILayout.IntPopup(tester.toGateIdx, gateNames,
                    nameIndexes, GUILayout.MaxWidth(130));

                tester.SetToGate(gateIndex, tester.toGateIdx == -1 ? null : gateDatabase.Areas[tester.toGateIdx]);

                if (tester.toGate == null && serializedObject.FindProperty("GoToScene") is SerializedProperty sceneProperty)
                    HandyFields.HardPropertyField("Scene Name: ", sceneProperty);
                else if (tester.toGateIdx < gateDatabase.Areas.Count && tester.toGateIdx > 0)
                    tester.SetToGate(gateIndex, gateDatabase.Areas[tester.toGateIdx]);


                if (tester.toGate != null && tester.toGate.isTPFD)
                    EditorGUILayout.LabelField("TPFD Room", GUILayout.MaxWidth(100));
            }

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayInvestigationGates()
        {
            if (tester.toGate == null || tester.toGate.fromLocations.Count == 0)
                return;

            string toScene = "";

            int investigationGateIndex = -1;

            if (tester.toGate.invGates.Count > 0)
            {
                EditorGUILayout.LabelField("Inv Gate: ", GUILayout.MaxWidth(100));

                string[] investigationGateNames = ContainerUtil
                    .PrependedList(tester.toGate.invGates.Select(invGate => invGate.sceneName).ToArray(), "None");

                if (investigationGateNames.Length > 0)
                {
                    int[] nameIndexes = ContainerUtil.Iota(investigationGateNames.Length, -1);

                    investigationGateIndex = EditorGUILayout.IntPopup(tester.invGateIdx, investigationGateNames,
                    nameIndexes, GUILayout.MaxWidth(130));

                    toScene = tester.invGateIdx == -1 ?
                        "" : tester.toGate.invGates[tester.invGateIdx].sceneName;
                }
            }

            tester.SetInvestigationScene(toScene, investigationGateIndex);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFromGate()
        {
            if (tester.toGate == null || tester.toGate.fromLocations.Count == 0)
                return;

            var locationList = tester.toGate.fromLocations;

            EditorGUILayout.LabelField("Coming From: ", GUILayout.MaxWidth(100));

            var fromGateNames = ContainerUtil
                .PrependedList(locationList.Select(fromGate => fromGate.gateFrom.ToAreaName).ToArray(), "None");

            int fromGateIndex = tester.fromGateIdx;

            if (fromGateNames.Length > 0)
                fromGateIndex = EditorGUILayout.IntPopup(tester.fromGateIdx, fromGateNames,
                ContainerUtil.Iota(fromGateNames.Length, -1), GUILayout.MaxWidth(130));

            bool validFromGateIndex =
                fromGateIndex < locationList.Count &&
                fromGateNames.Length != 0 &&
                fromGateIndex != -1;

            tester.SetFromGate(fromGateIndex, validFromGateIndex ? locationList[fromGateIndex] : null);

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDefaultFromGate()
        {
            if (tester.fromGate == null)
                using (new EditorGUILayout.HorizontalScope())
                    tester.SetPositionAndRotation(HandyFields.Vector3Field("Position: ", tester.position),
                        HandyFields.Vector3Field("Rotation: ", tester.rotation));

            GUILayout.Space(10);
        }

        #endregion

        #region Display Point In Time

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPointInTime()
        {
            if (tester.toGate == null || !DisplayingValidGameMoment())
                DisplayDefaultMoment();

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDefaultMoment()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Any Moment: ", GUILayout.Width(100));

                string[] objectiveNames = progressionDatabase.GetObjectiveNames(tester.chapterIndex);

                if (objectiveNames != null && objectiveNames.Length != 0)
                {
                    int[] nameIndexes = ContainerUtil.Iota(objectiveNames.Length, 0);

                    tester.SetObjectiveIndex(EditorGUILayout.IntPopup(tester.objectiveIndex, objectiveNames,
                    nameIndexes, GUILayout.MaxWidth(130)));

                    tester.SetObjective(progressionDatabase.Chapters[tester.chapterIndex].Objectives[tester.objectiveIndex]);
                }
                else
                    EditorGUILayout.LabelField("No Available Objectives for that chapter", GUILayout.Width(100));
            }

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DisplayingValidGameMoment()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (!DisplayBuilder(out RoomStateBuilder builder))
                    return false;

                if (!DisplaySection(builder, out RoomStateSection section))
                    return false;

                DisplayObjectiveInSection(section);

                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual bool DisplayBuilder(out RoomStateBuilder builder)
        {
            string builderName = tester.GoToScene != "" ? tester.GoToScene : tester.toGate.ToAreaName;

            builder = null;

            RoomStateBuilders ??= BuilderUtility.FindAssetsByType<RoomStateBuilder>();

            if (RoomStateBuilders.Count == 0)
                EditorGUILayout.LabelField("Couldn't find room builders");

            builder = RoomStateBuilders.Find(foundBuilder => foundBuilder.name == builderName);

            if (builder == null)
                EditorGUILayout.LabelField("Couldn't find builder with Name: " + builderName);

            return builder != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        protected virtual bool DisplaySection(RoomStateBuilder builder, out RoomStateSection section)
        {
            section = null;

            if (builder == null || builder.Sections.Count == 0)
                return false;

            bool chapterIndexValid = tester.chapterIndex < progressionDatabase.Chapters.Count && tester.chapterIndex >= 0;

            bool chapterIndexMatches = builder.Sections
                .Any(roomStateSection =>
                progressionDatabase.GetChapterIndex(roomStateSection.Chapter) == tester.chapterIndex);

            if (chapterIndexValid && chapterIndexMatches)
                section = builder.Sections[tester.chapterIndex];

            if (section == null)
                EditorGUILayout.LabelField("Couldn't find builds of room at the specified chapter");

            if (section.Rooms.Count == 0)
                EditorGUILayout.LabelField("Section had no Room Saves");

            return section != null && section.Rooms.Count != 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="section"></param>
        protected virtual void DisplayObjectiveInSection(RoomStateSection section)
        {
            EditorGUILayout.LabelField("At Moment: ", GUILayout.Width(100));

            string[] rooms = section.Rooms.Select(roomStateData => roomStateData.progressionValue.Description).ToArray();

            int[] roomIndexes = ContainerUtil.Iota(rooms.Length, 0);

            tester.SetObjectiveIndex(EditorGUILayout.IntPopup(tester.objectiveIndex, rooms,
                roomIndexes, GUILayout.MaxWidth(130)));

            if (section.Rooms.Count > 0 && tester.objectiveIndex < section.Rooms.Count)
                tester.SetObjective(section.Rooms[tester.objectiveIndex].progressionValue);
        }

        #endregion
    }
}
#endif