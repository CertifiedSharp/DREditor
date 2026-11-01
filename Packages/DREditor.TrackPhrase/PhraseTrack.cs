//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if CINEMACHINE
using Cinemachine;
using DREditor.Cameras;
using DREditor.Minigames.Debates;
using DREditor.Processables;
using UnityEngine;

namespace DREditor.TrackPhrase
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPhraseHandlerComponent) + ", " + nameof(IPhraseShatterListener) + ", " + nameof(IPhraseResetter) +
        ", " + nameof(IPrioritizable) +
        "\n\n")]
    public class PhraseTrack : MonoBehaviour, IPhraseHandlerComponent, IPhraseShatterListener, IPhraseResetter, IPrioritizable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int priority = 1;


        public int Priority { get => priority; set => priority = value; }


        /// <summary>
        ///
        /// </summary>
        protected TrackPath trackPath;

        /// <summary>
        ///
        /// </summary>
        protected DollyCartNSD cart;

        /// <summary>
        ///
        /// </summary>
        protected CinemachinePath path;

        /// <summary>
        ///
        /// </summary>
        protected bool usingTrack = false;

        /// <summary>
        ///
        /// </summary>
        protected GameObject pathRef;

        /// <summary>
        ///
        /// </summary>
        protected Transform cameraTransform;


        protected virtual void Awake() => cameraTransform = FindAnyObjectByType<DRTrialCamera>().transform;


        /// <inheritdoc/>
        public virtual void StartPhrase<T>(T phrase) where T : PhraseBase
        {
            foreach (var animation in phrase.animations)
                if (animation is TrackPath trackPath)
                    this.trackPath = trackPath;

            if (trackPath != null && trackPath.Points.Length > 0)
                StartAnimation();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartAnimation()
        {
            InitializeCart();

            InitializePath();

            PlayCart();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeCart()
        {
            GameObject parent = transform.parent.gameObject;

            if (parent.TryGetComponent<DollyCartNSD>(out var cart))
            {
                this.cart = cart;

                cart.SetPath(null);

                cart.SetSpeed(0);

                cart.SetPosition(0);
            }
            else
                this.cart = parent.AddComponent<DollyCartNSD>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePath()
        {
            pathRef = new GameObject();

            GameObject ch = new();

            ch.transform.parent = pathRef.transform;

            path = ch.AddComponent<CinemachinePath>();

            path.gameObject.transform.localPosition = trackPath.Position;

            Vector4 rot = trackPath.Rotation;

            path.gameObject.transform.localEulerAngles = new(rot.x, rot.y, rot.z);

            pathRef.transform.SetPositionAndRotation(new(0, cameraTransform.position.y, 0),
                new Quaternion(0, cameraTransform.rotation.y, 0, cameraTransform.rotation.w));

            transform.localPosition = new(0, -1, 0);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlayCart()
        {
            cart.SetPosition(0);

            cart.SetPositionUnits(CinemachinePathBase.PositionUnits.Normalized);

            cart.SetCurve(trackPath.SpeedCurve);

            cart.SetUseCurve(true);

            path.m_Waypoints = trackPath.Points;

            cart.SetPath(path);

            cart.SetSpeed(1);

            cart.enabled = true;

            usingTrack = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUsingTrack() => usingTrack;


        /// <inheritdoc/>
        public virtual void OnShattered()
        {
            if (usingTrack)
                transform.parent.gameObject.GetComponent<DollyCartNSD>().enabled = false;
        }

        /// <inheritdoc/>
        public virtual void Reset()
        {
            if (transform.parent.gameObject.TryGetComponent<DollyCartNSD>(out var cart))
                Destroy(cart);

            if (pathRef != null)
                Destroy(pathRef);

            trackPath = null;

            usingTrack = false;
        }
    }
}
#endif