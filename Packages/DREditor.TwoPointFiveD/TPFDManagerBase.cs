//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TPFDManagerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool reset = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setProtag = true;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float LAngle = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float RAngle = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float TAngle = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float BAngle = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float Distance = 10;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 CharPosition;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float InitialHAngle = 0f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float InitialVAngle = 0f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 ProtagPosition;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 ProtagRotation;


        /// <summary>
        ///
        /// </summary>
        protected PnCCamera pncCamera;


        /// <summary>
        ///
        /// </summary>
        public virtual void Start()
        {
            InitializeTPFD();

            PopulatePNCCamera();

            if (setProtag)
                SetProtagCameraPosition();
        }

        /// <summary>
        ///
        /// </summary>
        public abstract void InitializeTPFD();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PopulatePNCCamera()
        {
            pncCamera.ClearPosition();

            SetPnCCamera();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SetPnCCamera()
        {
            pncCamera.SetAngles(LAngle, RAngle, TAngle, BAngle);

            pncCamera.SetPositions(Distance, CharPosition);

            pncCamera.SetInitialAngles(InitialHAngle, InitialVAngle);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void SetProtagCameraPosition();

        
        /// <summary>
        ///
        /// </summary>
        protected virtual void Update()
        {
            CheckAllBoundaries();

            if (debugMode)
                SetPnCCamera();

            if (reset)
                PopulatePNCCamera();

            if (debugMode && setProtag)
                SetProtagCameraPosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void CheckAllBoundaries();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual float GetInitialHAngle() => InitialHAngle;

        /// <summary>
        ///
        /// </summary>
        public virtual void StartEarlySet()
        {
            Start();

            SetPnCCamera();
        }
    }
}