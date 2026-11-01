//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class SceneEventOnStart : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected SceneEvent sceneEvent = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => sceneEvent.Raise();
    }
}