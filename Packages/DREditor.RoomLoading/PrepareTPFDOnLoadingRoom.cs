//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Gates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PrepareTPFDOnLoadingRoom : MonoBehaviour
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
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent prepareTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onLoadingRoom = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => onLoadingRoom.Response.AddListener(PrepareForTPFD);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => onLoadingRoom.Response.RemoveListener(PrepareForTPFD);

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareForTPFD()
        {
            Gate gate = gateDatabase.GetArea(SceneManager.GetActiveScene().name);

            if (gate != null && gate.isTPFD)
            {
                if (!inTPFD.Value)
                    inTPFD.SetValue(true);

                prepareTPFD.Raise();
            }
        }
    }
}