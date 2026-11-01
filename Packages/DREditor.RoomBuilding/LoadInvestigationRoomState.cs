//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.Progressing;
using DREditor.Services;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class LoadInvestigationRoomState : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueEventString = "LoadInvestigationRoom";


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => StartListening();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartListening() => DialogueEventSystem.StartListening(dialogueEventString, LoadInvestigationInstance);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => StopListening();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopListening() => DialogueEventSystem.StopListening(dialogueEventString, LoadInvestigationInstance);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void LoadInvestigationInstance(object o = null)
        {
            ServiceLocator.GetService<RoomManagerBase>().UnloadAll(false);

            ServiceLocator.GetService<RoomManagerBase>().LoadRoomState((Objective)o, null);
        }
    }
}