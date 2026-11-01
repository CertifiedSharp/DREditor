//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.RoomBuilding.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class RoomBuilderContainsObjective : RoomBuilderScanWizard
    {
        [MenuItem("DREditor/Utility/Scan/Room Builders Containing Objective")]
        public static void CreateWizard()
            => DisplayWizard<ScanRoomBuilders>("Scan Room Builders Containing Objective", "Find Builders");


        protected virtual void OnWizardCreate() => HasObjective();


        /// <summary>
        ///
        /// </summary>
        protected virtual void HasObjective()
        {
            List<RoomStateBuilder> builders = BuilderUtility.FindAssetsByType<RoomStateBuilder>();

            foreach (RoomStateBuilder builder in builders)
            {
                builder.GetSectionWithObjective(chapter, objective);

                if (builder != null)
                    UnityEngine.Debug.Log(builder.name + $" has a Room Save for {chapter.title}: {objective.Description}");
            }
        }
    }
}
#endif