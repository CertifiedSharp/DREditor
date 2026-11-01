//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomLoadComponent) + ", " + nameof(IRoomLoadRoutine) +
        "\n\n")]
    public class CheckThreeDModeDuringLoad : RoomLoadComponent, IRoomLoadRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("GDB")]
        [SerializeField] protected GateDatabase gateDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inThreeDMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <inheritdoc/>
        public virtual IEnumerator OnRoomLoadRoutine()
        {
            CheckThreeDMode();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckThreeDMode()
        {
            // Added 8-27-2024 because going dorm hallway ch 1 free time, laundromat tp, then leaving, 
            // always had sprite billboard looking where the player last was.

            Gate changerDestination = ServiceLocator.GetService<RoomChangerBase>().GetDestination();

            Gate destination = changerDestination != null ? 
                changerDestination : gateDatabase.GetArea(SceneManager.GetActiveScene().name);

            if (destination != null && !destination.isTPFD && !isLoadingFile.Value)
                inThreeDMode.SetValue(true);
        }
    }
}