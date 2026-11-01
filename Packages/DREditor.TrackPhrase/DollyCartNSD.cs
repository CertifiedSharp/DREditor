//Editing Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if CINEMACHINE
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using static Cinemachine.CinemachineDollyCart;

namespace DREditor.TrackPhrase
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [DisallowMultipleComponent]
    public class DollyCartNSD : MonoBehaviour
    {
        /// <summary></summary>
        [Tooltip("")]
        [SerializeField] protected CinemachinePathBase m_Path;

        /// <summary></summary>
        [Tooltip("")]
        [SerializeField] protected UpdateMethod m_UpdateMethod = UpdateMethod.Update;

        /// <summary></summary>
        [Tooltip("")]
        [SerializeField] protected CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;

        /// <summary></summary>
        [Tooltip("")]
        [FormerlySerializedAs("m_Velocity")]
        [SerializeField] protected float m_Speed;

        /// <summary></summary>
        [Tooltip("")]
        [FormerlySerializedAs("m_CurrentDistance")]
        [SerializeField] protected float m_Position;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useCurve = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool reset = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AnimationCurve curve = new(new Keyframe[]
        {
            new(0,0), new(1, 1)
        });


        /// <summary>
        ///
        /// </summary>
        /// <param name="m_Path"></param>
        public virtual void SetPath(CinemachinePathBase m_Path) => this.m_Path = m_Path;

        /// <summary>
        ///
        /// </summary>
        /// <param name="m_Speed"></param>
        public virtual void SetSpeed(float m_Speed) => this.m_Speed = m_Speed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="m_Position"></param>
        public virtual void SetPosition(float m_Position) => this.m_Position = m_Position;

        /// <summary>
        ///
        /// </summary>
        /// <param name="m_PositionUnits"></param>
        public virtual void SetPositionUnits(CinemachinePathBase.PositionUnits m_PositionUnits)
            => this.m_PositionUnits = m_PositionUnits;

        /// <summary>
        ///
        /// </summary>
        /// <param name="useCurve"></param>
        public virtual void SetUseCurve(bool useCurve) => this.useCurve = useCurve;

        /// <summary>
        ///
        /// </summary>
        /// <param name="curve"></param>
        public virtual void SetCurve(AnimationCurve curve) => this.curve = curve;


        /// <summary>
        ///
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (m_UpdateMethod == UpdateMethod.FixedUpdate)
                SetCartPosition(m_Position + m_Speed * Time.deltaTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void Update()
        {
            if (reset)
            {
                m_Position = useCurve ? 0.1f : 0;

                reset = false;

                return;
            }

            float speed = Application.isPlaying ? m_Speed : 0;

            if (useCurve)
                speed = curve.Evaluate(m_Position);

            if (m_UpdateMethod == UpdateMethod.Update)
                SetCartPosition(m_Position + speed * Time.deltaTime);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (!Application.isPlaying)
                SetCartPosition(m_Position);
            else if (m_UpdateMethod == UpdateMethod.LateUpdate)
                SetCartPosition(m_Position + m_Speed * Time.deltaTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="distanceAlongPath"></param>
        protected virtual void SetCartPosition(float distanceAlongPath)
        {
            if (m_Path != null)
            {
                m_Position = m_Path.StandardizeUnit(distanceAlongPath, m_PositionUnits);

                transform.SetPositionAndRotation(m_Path.EvaluatePositionAtUnit(m_Position, m_PositionUnits),
                    m_Path.EvaluateOrientationAtUnit(m_Position, m_PositionUnits));
            }
        }
    }
}
#endif