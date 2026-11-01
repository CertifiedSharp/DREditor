//Camera Script for DREditor Author: SeleniumSoul
//Rework Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.Cameras
{
    using Debug = UnityEngine.Debug;
    [AddComponentMenu("DREditor/Cameras/Point and Click Camera")]
    public class PnCCamera : SceneEventListener
    {
        //Public parameters

        [SerializeField] protected bool controllable = true;
        public bool Controllable { get => controllable; set => controllable = value; }
        public Vector3 CharPosition = new(0f, 1f, 0f);
        [SerializeField] protected float MaxLAngle = 30f;
        [SerializeField] protected float MaxRAngle = 30f;
        [SerializeField] protected float MaxTAngle = 20f;
        [SerializeField] protected float MaxBAngle = 20f;
        [SerializeField] protected float CamSpeed = 3f;
        [SerializeField] protected float InitialHAngle = 0f;
        [SerializeField] protected float InitialVAngle = 0f;
        [SerializeField] protected float CamDistance = 10f;

        //Focus on Object
        public GameObject TargetObject;
        public Vector3 TargetAimRotation;
        public float TargetCamDistance = 5f;

        //variables for calculation
        public float CamHAngle, CamVAngle;
        protected Vector3 TargetFocus;
        protected Quaternion FinalAimRotation;
        protected Vector3 FinalCamPosition;

        protected int invertXInt;

        [Tooltip("")]
        [SerializeField] protected InputActionReference tpfdMove;

        [SerializeField] ValueWithEvent<bool, BoolEvent> invertX = null;



        
        void Update()
        {
            if (Controllable)
                UpdateOnControlled();
            else if (TargetObject != null)
            {
                SetTargetFocus(TargetObject.transform.position);

                FinalAimRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(TargetAimRotation), Time.deltaTime * 30f);

                FinalCamPosition = Vector3.Lerp(transform.position, TargetFocus - (transform.forward * TargetCamDistance), Time.deltaTime * 30f);
            }
        }


        protected virtual void UpdateOnControlled()
        {
            ClearTarget();

            invertXInt = invertX.Value ? -1 : 1;

            Calculate(updateZ: false);
        }

        public Vector3 GetCalculatedPosition(bool updateZ = false)
        {
            OnPrecalculation();

            Calculate(updateZ);

            return FinalCamPosition;
        }

        protected virtual void OnPrecalculation()
        {
            ClearPosition();

            ClearTarget();
        }

        public Quaternion GetCalculatedRotation()
        {
            GetCalculatedPosition(updateZ: true);

            return FinalAimRotation;
        }


        protected virtual void Calculate(bool updateZ = false)
        {
            CamHAngle += Mathf.Clamp(tpfdMove.action.ReadValue<Vector2>().x * invertXInt, -1, 1) * (CamSpeed * 10) * Time.deltaTime;

            CamVAngle += Mathf.Clamp(tpfdMove.action.ReadValue<Vector2>().y, -1, 1) * (CamSpeed * 10) * Time.deltaTime;

            CamHAngle = Mathf.Clamp(CamHAngle, -MaxLAngle, MaxRAngle);

            CamVAngle = Mathf.Clamp(CamVAngle, -MaxBAngle, MaxTAngle);

            FinalAimRotation = Quaternion.Euler(-CamVAngle + InitialVAngle, CamHAngle + InitialHAngle, 0);

            Vector3 direction = updateZ ? new Vector3(0, 0, 1) : transform.forward;

            FinalCamPosition = (TargetFocus - new Vector3(0, CamVAngle / 30f)) - (direction * CamDistance);
        }


        public void FocusCamera(string TarObj)
        {
            FocusCamera(GameObject.Find(TarObj), Vector3.zero, 8f);

            Debug.Log($"REPORT: {nameof(PnCCamera)}: Focusing on " + TargetObject.name + "using a string.");
        }

        public void FocusCamera(GameObject target, Vector3 rotation, float distance)
        {
            ControlSwitch(false);

            SetTargetObject(target);

            TargetAimRotation = rotation;

            TargetCamDistance = distance;

            Debug.Log($"REPORT: {nameof(PnCCamera)}: Focusing on a spot manually.");
        }

        public void ControlSwitch(bool option)
        {
            Controllable = option;
        }

        

        protected virtual void ClearTarget()
        {
            SetTargetObject(null);

            SetTargetFocus(CharPosition);
        }

        protected virtual void SetTargetObject(GameObject target) => TargetObject = target;
        protected virtual void SetTargetFocus(Vector3 target) => TargetFocus = target;

        public void ClearPosition()
        {
            CamHAngle = 0;

            CamVAngle = 0;
        }

        protected virtual void LateUpdate() => transform.SetPositionAndRotation(FinalCamPosition, FinalAimRotation);

        public override void OnEventRaised()
        {
            base.OnEventRaised();

            enabled = false;
        }

        public void SetInitialAngles(float InitialHAngle, float InitialVAngle)
        {
            this.InitialHAngle = InitialHAngle;

            this.InitialVAngle = InitialVAngle;
        }

        public virtual void SetPositions(float CamDistance, Vector3 CharPosition)
        {
            this.CamDistance = CamDistance;

            this.CharPosition = CharPosition;
        }

        public virtual void SetAngles(float MaxLAngle, float MaxRAngle, float MaxTAngle, float MaxBAngle)
        {
            this.MaxLAngle = MaxLAngle;

            this.MaxRAngle = MaxRAngle;

            this.MaxTAngle = MaxTAngle;

            this.MaxBAngle = MaxBAngle;
        }

        public virtual Vector3 GetFinalPosition() => FinalCamPosition;
    }
}