//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Doors;
using DREditor.EventObjects;
using DREditor.FPC;
using DREditor.TwoPointFiveD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SubAreaActivationHandler : SubAreaManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoxCollider subAreaCollider;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TPFDManager tpfdManager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectGroup objectGroup = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool TPFDMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float panToBodyTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";


        /// <summary>
        ///
        /// </summary>
        public event Action OnHandlerFinished;


        /// <summary>
        ///
        /// </summary>
        protected ControlMonobehaviours playerSystem;

        /// <summary>
        ///
        /// </summary>
        protected TPFDManager[] managers;

        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera mainCamera;


        /// <inheritdoc/>
        protected override void Awake()
        {
            if (AlreadyInTPFD())
            {
                tpfdManager.enabled = false;

                tpfdManager.InitializeTPFD();
            }

            base.Awake();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AlreadyInTPFD() => TPFDMode;

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag(mainCameraTag).GetComponent<UnityEngine.Camera>();

            playerSystem = mainCamera.transform.root.GetComponent<ControlMonobehaviours>();

            managers = FindObjectsByType<TPFDManager>().Where(manager => manager.enabled).ToArray();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="subAreaData"></param>
        public abstract void Initialize(object subAreaData);
        
        /// <summary>
        ///
        /// </summary>
        public abstract void BeginActivationState();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<IDoor> GetTPFDDoors()
            => FindObjectsByType<MonoBehaviour>().OfType<IDoor>().Where(door => door.IsTPFD()).ToList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="duration"></param>
        /// <param name="bufferTime"></param>
        protected virtual IEnumerator MoveCameraToPosition(Vector3 position, Vector3 rotation, float duration, float bufferTime = 0)
        {
#if DOTWEEN
            mainCamera.transform.DORotate(rotation, duration);

            mainCamera.transform.DOMove(position, duration);
#endif
            yield return new WaitForSeconds(duration + bufferTime);
        }

        /// <summary>
        ///
        /// </summary>
        public abstract void EndOfActivationHandler();

        /// <summary>
        ///
        /// </summary>
        protected virtual void FinishHandler() => OnHandlerFinished?.Invoke();
    }
}