//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class TPFDManager : TPFDManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string playerTag = "Player";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> dialogueOnDestroyEvents = new() { "ResetProtagCamera" };


        /// <summary>
        ///
        /// </summary>
        protected GameObject mainCamera = null;

        /// <summary>
        ///
        /// </summary>
        protected GameObject protag;

        /// <summary>
        ///
        /// </summary>
        protected GameObject player;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<ITPFDListener> listeners;


        /// <inheritdoc/>
        public override void Start()
        {
            if (mainCamera != null)
                return;

            base.Start();
        }

        /// <inheritdoc/>
        public override void InitializeTPFD()
        {
            mainCamera = GameObject.FindWithTag(mainCameraTag);

            pncCamera = mainCamera.GetComponent<PnCCamera>();

            player = GameObject.FindWithTag(playerTag);

            protag = player.GetComponentInChildren<UnityEngine.Camera>().gameObject;

            listeners = FindObjectsByType<MonoBehaviour>().OfType<ITPFDListener>();
        }

        /// <inheritdoc/>
        protected override void SetProtagCameraPosition()
        {
            protag.transform.localPosition = ProtagPosition;

            protag.transform.localEulerAngles = ProtagRotation;
        }


        /// <inheritdoc/>
        protected override void CheckAllBoundaries()
        {
            CheckBoundary(pncCamera.CamVAngle == TAngle, 0);

            CheckBoundary(pncCamera.CamVAngle == -BAngle, 1);

            CheckBoundary(pncCamera.CamHAngle == -LAngle, 2);

            CheckBoundary(pncCamera.CamHAngle == RAngle, 3);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hittingBoundary"></param>
        /// <param name="directionIndex"></param>
        protected virtual void CheckBoundary(bool hittingBoundary, int directionIndex)
        {
            if (hittingBoundary)
                OnBoundaryEntered(directionIndex);
            else
                OnBoundaryExited(directionIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directionIndex"></param>
        protected virtual void OnBoundaryEntered(int directionIndex)
        {
            foreach (var listener in listeners)
                listener.OnEnteredBoundary(directionIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directionIndex"></param>
        protected virtual void OnBoundaryExited(int directionIndex)
        {
            foreach (var listener in listeners)
                listener.OnExitedBoundary(directionIndex);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy()
        {
            foreach (string dialogueEvent in dialogueOnDestroyEvents)
                DialogueEventSystem.TriggerEvent(dialogueEvent);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Move Protag Camera To Protag Position")]
        public void ContextSetProtagCamera() => SetProtagCameraPosition();
    }
}