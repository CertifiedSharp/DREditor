//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class EvaluateCamerasStates : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inVideoMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<BoolWithEvent> cameras = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => StartListeningToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartListeningToEvents()
        {
            inTrialMode.AddListener(EvaluateInTrialState);

            inCGMode.AddListener(EvaluateInCGState);

            inVideoMode.AddListener(EvaluateInCGState);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => StopListeningToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopListeningToEvents()
        {
            inTrialMode.RemoveListener(EvaluateInTrialState);

            inCGMode.RemoveListener(EvaluateInCGState);

            inVideoMode.RemoveListener(EvaluateInCGState);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inTrial"></param>
        protected virtual void EvaluateInTrialState(bool inTrial) => EnableCamerasTo(!inTrial);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCG"></param>
        protected virtual void EvaluateInCGState(bool inCG)
        {
            if (!inTrialMode.Value)
                EnableCamerasTo(!inCG);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        protected virtual void EnableCamerasTo(bool to)
        {
            foreach (BoolWithEvent camera in cameras)
                if (camera != null)
                    camera.SetValue(to);
                else
                    UnityEngine.Debug.Log($"{ToString()}: Canvases and Cameras states were not changed because " +
                    "a dialogue camera has not been found.");
        }
    }
}