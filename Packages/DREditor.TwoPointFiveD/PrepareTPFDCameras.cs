//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PrepareTPFDCameras : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string prepareTPFDCameraEventString = "PrepareCamerasTPFD";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener prepareTPFD = null;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float angleThreshold = 180;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => prepareTPFD.Response.AddListener(PrepareForTPFD);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => prepareTPFD.Response.RemoveListener(PrepareForTPFD);

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareForTPFD()
        {
            foreach (TPFDManagerBase manager in FindObjectsByType<TPFDManagerBase>())
                if (InitializeValidActiveManager(manager))
                    break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        protected virtual bool InitializeValidActiveManager(TPFDManagerBase manager)
        {
            bool result = manager != null && manager.enabled;

            if (result)
            {
                manager.Start();

                DialogueEventSystem.TriggerEvent(prepareTPFDCameraEventString, manager.GetInitialHAngle() >= angleThreshold);
            }
            
            return result;
        }
    }
}