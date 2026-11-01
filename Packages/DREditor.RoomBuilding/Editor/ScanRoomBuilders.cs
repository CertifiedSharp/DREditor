//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.Progressing;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class ScanRoomBuilders : RoomBuilderScanWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Objective replacement;


        [MenuItem("DREditor/Utility/Scan/Replace Objective in Room Builders")]
        public static void CreateWizard() => DisplayWizard<ScanRoomBuilders>("Scan Builders", "Show Replacement");

        protected virtual void OnWizardCreate() => ChangeObjectiveSafely(replacement);


        /// <summary>
        ///
        /// </summary>
        /// <param name="replacement"></param>
        protected virtual void ChangeObjectiveSafely(Objective replacement)
        {
            AssetDatabase.StartAssetEditing();

            List<RoomStateBuilder> builders = BuilderUtility.FindAssetsByType<RoomStateBuilder>();

            foreach (RoomStateBuilder builder in builders)
                ReplaceObjectiveInBuilder(builder, replacement);

            progressionDatabase.Chapters[chapterIndex].Objectives[objectiveIndex] = replacement;

            EditorUtility.SetDirty(progressionDatabase.Chapters[chapterIndex]);

            AssetDatabase.StopAssetEditing();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="replacement"></param>
        protected virtual void ReplaceObjectiveInBuilder(RoomStateBuilder builder, Objective replacement)
        {
            RoomStateSection section = builder.GetSectionWithObjective(chapter, objective);

            if (section != null)
            {
                int sectionNum = builder.Sections.IndexOf(section);

                RoomStateData room = (RoomStateData)section.GetDataWithObjective(objective);

                int roomNum = section.Rooms.IndexOf(room);

                builder.Sections[sectionNum].Rooms[roomNum].SetProgressionValue((Objective)replacement.Clone());

                EditorUtility.SetDirty(builder);
            }
        }
    }
}
#endif