//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.RoomBuilding.Implementation.Actors;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class FindCharactersInRoomsWizard : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [TextArea]
        [Tooltip("")]
        [SerializeField] protected string text;


        [MenuItem("DREditor/Utility/Scan/Room Builders for a character")]
        public static void CreateWizard() 
            => DisplayWizard<FindCharactersInRoomsWizard>("Scan Room Builders for a character", "Find Builders");

        protected virtual void OnWizardCreate() => FindRoomStatesWithCharacter(text);


        /// <summary>
        ///
        /// </summary>
        /// <param name="characterName"></param>
        protected virtual void FindRoomStatesWithCharacter(string characterName)
        {
            UnityEngine.Debug.Log("Scanning All Room Saves with Character: " + characterName);

            var builders = BuilderUtility.FindAssetsByType<RoomStateBuilder>();

            foreach (var builder in builders)
                foreach (var section in builder.Sections)
                    foreach (var save in section.Rooms)
                        foreach (ActorStateData actorStateData in save.dataStates.Cast<ActorStateData>())
                            foreach (var actorData in actorStateData.datas)
                                if (actorData.aReference == characterName)
                                    UnityEngine.Debug.Log("Area: " + builder.name + " Chapter: " +
                                        section.Chapter.name + " Save: " + save.progressionValue.Description);

            UnityEngine.Debug.Log("Scan Complete");
        }
    }
}
#endif