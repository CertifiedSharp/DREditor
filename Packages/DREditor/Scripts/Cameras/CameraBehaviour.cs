//Author: Happy30
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.InputSystem;


namespace DREditor.Cameras
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] ValueWithEvent<bool, BoolEvent> drCam = null;
        public bool DR_Style => drCam != null?
            drCam.Value : DR_Cam;
        [SerializeField] bool DR_Cam = false;
        public Crouch CrouchModifier;
        public Headbobbing HeadbobbingModifier;
        [SerializeField] ValueWithEvent<bool, BoolEvent> headbobbing = null;
        public bool HeadbobbingEnabled => headbobbing != null?
            headbobbing.Value : Bobbing;
        [SerializeField] bool Bobbing = true;
        public FollowPlayer FollowPlayerModifier;
        public SmoothMouseLook MouseLookModifier;
        public CameraShake CameraShake;
        public Transform Player;
        public BoolWithEvent InCameraShakeMode;

        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        public bool IsMoving;
        private float _timer;
        private float TimeForNewShake;

        private Vector3 ShakeOffset;

        private Vector3 originalPos;
        public bool Fog;

        [Tooltip("")]
        [SerializeField] protected InputActionReference move;

        // Start is called before the first frame update
        void Start()
        {
            RenderSettings.fog = Fog;
            InCameraShakeMode.Value = false;
        }

        // Update is called once per frame
        void Update()
        {
            IsMoving = move.action.ReadValue<Vector2>() != Vector2.zero;

            UpdateFollowPlayer();

            UpdateMouseLook();

            UpdateHeadBobbing();

            UpdateCameraShake();

            UpdateEnabled();
        }


        protected virtual void UpdateFollowPlayer()
        {
            if (FollowPlayerModifier)
                transform.position = FollowPlayerModifier.GetPlayerPosition();
        }

        protected virtual void UpdateMouseLook()
        {
            if (MouseLookModifier.enabled)
            {
                if (DR_Style)
                {
                    if (!IsMoving)
                        transform.localRotation = MouseLookModifier.GetRotation();
                    else
                        transform.localRotation = MouseLookModifier.DRGetRotation();
                }
                else
                    transform.localRotation = MouseLookModifier.GetRotation();

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Player.localEulerAngles.y, transform.localEulerAngles.z);
            }
        }

        protected virtual void UpdateHeadBobbing()
        {
            if (HeadbobbingEnabled && HeadbobbingModifier.enabled)
                transform.position = transform.position + new Vector3(0, HeadbobbingModifier.GetYBobAmount(), 0);
        }

        protected virtual void UpdateCameraShake()
        {
            if (CameraShake && InCameraShakeMode.Value & CameraShake.Enabled)
            {
                TimeForNewShake = 2 - (CameraShake.Omega.Value * 2);

                _timer += Time.deltaTime;

                if (_timer > TimeForNewShake)
                {
                    originalPos = transform.localPosition;
                    ShakeOffset = CameraShake.GetRandomOffset();
                    _timer = 0f;
                }

                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + ShakeOffset, 0.2f * Time.deltaTime);

                // ETween.Step(transform.localPosition, originalPos + ShakeOffset, 0.2f);
            }
        }

        protected virtual void UpdateEnabled()
        {
            if (MouseLookModifier.InDialogue.Value)
                enabled = false;
        }

        public void CallLook()
        {
            transform.position = FollowPlayerModifier.GetPlayerNoLerp();
            if (DR_Style)
            {
                if (!IsMoving) transform.localRotation = MouseLookModifier.GetRotation();
                else transform.localRotation = MouseLookModifier.DRGetRotation(false);
            }
            else
            {
                transform.localRotation = MouseLookModifier.GetRotation(false);
            }

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Player.localEulerAngles.y, transform.localEulerAngles.z);
        }

        public void OnSetPlayerState() // Called from Scene Event Listener OnSetPlayerState Scene Event
        {
            if (playerSpawnPoint.Value.Pos != Vector3.zero)
                CallLook();
        }
    }
}