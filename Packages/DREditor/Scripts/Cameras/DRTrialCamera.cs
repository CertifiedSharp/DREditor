// Main Trial Camera Script for DREditor Author: SeleniumSoul
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Cameras
{
    [AddComponentMenu("DREditor/Cameras/DR Trial Camera")]
    public class DRTrialCamera : MonoBehaviour
    {
        public GameObject CameraPivot;
        public Animator CameraAnimator;

        [SerializeField] float DefaultRadius = 8.11f;

        [SerializeField] protected float seatFocus;
        public virtual float SeatFocus { get => seatFocus; set => seatFocus = value; }
        
        public float SeatRadius;

        public bool RadiusLock = true;

        [SerializeField] protected bool smoothFocus;

        public bool SmoothFocus { get => smoothFocus; set => smoothFocus = value; }

        public float SmoothTransitionTime = 10f;
        public int HeadmasterPosition = 0;
        public float HeamasterSeatHeightOffset = 12f;
        public float HeadmasterSeatDistance = 12f;
        public float[] CharHeightOffset = new float[16];
        private float AnchorAngle;
        private bool FocusOnHeadmaster = false;

        private bool Replace = false;
        private float HeightOverride;


        void Start()
        {
            if (CameraPivot == null)
            {
                CameraPivot = gameObject.transform.GetChild(0).gameObject;
            }
            if (CameraAnimator == null)
            {
                CameraAnimator = CameraPivot.GetComponent<Animator>();
            }

            AnchorAngle = 360f / CharHeightOffset.Length;
        }

        void Update()
        {
            RadiusLock = CameraPivot.GetComponent<RadiusLock>().Locked;
            if (FocusOnHeadmaster)
            {
                UpdatePositionAndRotation(new Vector3(0, HeamasterSeatHeightOffset, 0),
                    Quaternion.AngleAxis(HeadmasterPosition * AnchorAngle, Vector3.up));
            }
            else if (Replace)
            {
                //UnityEngine.Debug.LogWarning("USING REPLACE");
                UpdatePositionAndRotation(new Vector3(0, HeightOverride, 0), Quaternion.AngleAxis(SeatFocus * AnchorAngle, Vector3.up));
            }
            else
            {
                if (Mathf.RoundToInt(SeatFocus) >= CharHeightOffset.Length)
                    SeatFocus = 0;
                try
                {
                    UpdatePositionAndRotation(new Vector3(0, CharHeightOffset[Mathf.RoundToInt(SeatFocus)], 0), 
                        Quaternion.AngleAxis(SeatFocus * AnchorAngle, Vector3.up));
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("Camera Seat focus issue with: " + Mathf.RoundToInt(SeatFocus));
                }
            }
        }
        void LateUpdate()
        {
            if (RadiusLock)
            {
                if(FocusOnHeadmaster)
                {
                    CameraPivot.transform.localPosition = new Vector3(0, 0, HeadmasterSeatDistance);
                } else
                {
                    CameraPivot.transform.localPosition = new Vector3(0, 0, SeatRadius);
                }
            } 
        }

        void UpdatePositionAndRotation(Vector3 targetPosition, Quaternion targetRotation)
        {
            if (SmoothFocus)
                transform.SetPositionAndRotation(
                    Vector3.Lerp(transform.position, targetPosition, SmoothTransitionTime * Time.deltaTime),
                    Quaternion.Lerp(transform.rotation, targetRotation, SmoothTransitionTime * Time.deltaTime));
            else
                transform.SetPositionAndRotation(targetPosition, targetRotation);
        }

        public void SetHeadmasterFocus(bool value)
        {
            FocusOnHeadmaster = value;
        }

        public bool IsFocusedOnHeadmaster()
        {
            return FocusOnHeadmaster;
        }

        public void ChangeFocus(float charnum)
        {
            SeatFocus = charnum;
        }

        public void TriggerAnim(int id)
        {
            CameraAnimator.SetTrigger(id);
        }

        public void TriggerAnim(string tag)
        {
            CameraAnimator.SetTrigger(tag);
        }
        public void ResetTrigger(string tag) => CameraAnimator.ResetTrigger(tag);
        public void TriggerAnim(float seat, string animname = "noCamAnim")
        {
            SeatFocus = seat;
            CameraAnimator.SetTrigger(animname);
        }

        public void SetOverride(bool to)
        {
            Replace = to;
        }
        public void SetDefaultValues()
        {
            SetOverride(false);

            SeatRadius = DefaultRadius;
        }

        public virtual void ApplyOverrides(float height, float distance, float seatFocus, bool dontPan)
        {
            HeightOverride = height;

            SeatRadius = distance;

            if (!dontPan)
                SeatFocus = seatFocus;

            SetOverride(true);
        }
    }
}