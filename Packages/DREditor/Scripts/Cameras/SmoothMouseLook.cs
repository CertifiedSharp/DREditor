//Author: Happy30
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.Cameras
{
    public class SmoothMouseLook : MonoBehaviour
    {
        [SerializeField] Vector2 _mouseAbsolute;
        [SerializeField] Vector2 _smoothMouse;

        [SerializeField] ValueWithEvent<bool, BoolEvent> invertX = null;
        public bool XInvert => invertX != null? 
            invertX.Value : InvertX;

        [SerializeField] bool InvertX = false;


        [SerializeField] ValueWithEvent<bool, BoolEvent> invertLook = null;

        public bool LookInvert => invertLook != null?
            invertLook.Value : InvertLook;

        [SerializeField] bool InvertLook = false;

        public Vector2 clampInDegrees = new Vector2(360, 180);
        public bool lockCursor;
        public Vector2 sensitivity = new Vector2(2, 2);
        public Vector2 smoothing = new Vector2(3, 3);
        public Vector2 targetDirection;
        public Vector2 targetCharacterDirection;

        public BoolWithEvent InDialogue;

        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        [Tooltip("")]
        [SerializeField] protected InputActionReference look;

        // Assign this if there's a parent object controlling motion, such as a Character Controller.
        // Yaw rotation will affect this object instead of the camera if set.
        public GameObject characterBody;


        void Start()
        {
            // Set target direction to the camera's initial orientation.
            targetDirection = transform.localRotation.eulerAngles;

            // Set target direction for the character body to its inital state.
            if (characterBody)
                targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        }

        public void SetSenstitivity(float nextSensitivity)
        {
            nextSensitivity--;
            sensitivity = new Vector2(nextSensitivity, nextSensitivity);
        }
        public Vector2 GetAbsolute() => _mouseAbsolute;
        public Vector2 GetSmooth() => _smoothMouse;
        //const Vector2 defaultSmooth = ;

        public void SetCore()
        {
            ClearCorePos();

            if (playerSpawnPoint.Value.Pos != Vector3.zero)
                SetCorePos(new Vector2(playerSpawnPoint.Value.Rot.eulerAngles.y, characterBody.transform.localEulerAngles.x));
            else
                transform.localEulerAngles
                    = new Vector2(characterBody.transform.localEulerAngles.x, playerSpawnPoint.Value.Rot.eulerAngles.y);
        }

        public void SetCorePos(Vector2 abs, Vector2? smooth = null)
        {
            ClearCorePos();

            smooth ??= new Vector2(1.401298e-45f, 1.401298e-45f);

            SetMice(abs, smooth);
        }

        public void ClearCorePos() => SetMice(Vector2.zero, Vector2.zero);

        protected virtual void SetMice(Vector2 abs, Vector2? smooth = null)
        {
            _mouseAbsolute = abs;

            _smoothMouse = (Vector2)smooth;
        }

        public Quaternion GetRotation(bool regular = true)
        {
            LockCursor();

            // Get raw mouse input for a cleaner reading on more sensitive mice.
#if ENABLE_INPUT_SYSTEM
            var mouseDelta = look.action.ReadValue<Vector2>();
#elif ENABLE_LEGACY_INPUT_MANAGER
			var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
#endif
            if (XInvert)
                mouseDelta.x = -mouseDelta.x;

            if (LookInvert)
                mouseDelta.y = -mouseDelta.y;

            var newSensiv = sensitivity;

            if (InDialogue.Value)
            {
                newSensiv /= 20;
            }

            if (!regular)
            {
                mouseDelta.x = 0;

                mouseDelta.y = 0;
            }

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(newSensiv.x * smoothing.x, newSensiv.y * smoothing.y));

            UpdateAbsolute(mouseDelta);

            UpdateTransform();

            var targetOrientation = Quaternion.Euler(targetDirection);

            return Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;
        }
        public Quaternion DRGetRotation(bool regular = true)
        {
            LockCursor();

            // Get raw mouse input for a cleaner reading on more sensitive mice.
#if ENABLE_INPUT_SYSTEM
            var mouseDelta = look.action.ReadValue<Vector2>();
#elif ENABLE_LEGACY_INPUT_MANAGER
			var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
#endif

            if (!regular)
            {
                mouseDelta.x = 0;

                mouseDelta.y = 0;
            }

            if (XInvert)
                mouseDelta.x = -mouseDelta.x;

            if (LookInvert)
                mouseDelta.y = -mouseDelta.y;

            var newSensiv = sensitivity;

            if (InDialogue.Value)
            {
                newSensiv /= 20;
            }

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(newSensiv.x * smoothing.x, newSensiv.y * smoothing.y));

            UpdateAbsolute(mouseDelta);

            UpdateTransform();

            _mouseAbsolute.y = Mathf.Lerp(_mouseAbsolute.y, 0f, 0.2f);

            var targetOrientation = Quaternion.Euler(targetDirection);

            return Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;
        }

        protected virtual void UpdateAbsolute(Vector2 mouseDelta)
        {
            // Interpolate mouse movement over time to apply smoothing delta.
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            // Find the absolute mouse movement value from point zero.
            _mouseAbsolute += _smoothMouse;

            // Clamp and apply the local x value first, so as not to be affected by world transforms.
            if (clampInDegrees.x < 360)
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

            // Then clamp and apply the global y value.
            if (clampInDegrees.y < 360)
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        }

        protected virtual void UpdateTransform()
        {
            if (characterBody)
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
                characterBody.transform.localRotation = yRotation * Quaternion.Euler(targetCharacterDirection);
            }
            else
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
                transform.localRotation *= yRotation;
            }
        }

        protected virtual void LockCursor()
        {
            // Ensure the cursor is always locked when set
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        [Tooltip("")]
        [SerializeField] protected TransformWithEvent mouseAbs = null;

        public virtual void OnSetPlayerState() // Called from Scene Event Listener OnSetPlayerState Scene Event
        {
            if (isLoadingFile.Value)
                SetCorePos(mouseAbs.Value.Pos);
            else
                SetCore();
        }

        public void ClearPlayerSpawnTransform() // Called from Scene Event Listener StartedNewGame Scene Event
        {
            playerSpawnPoint.Value.Clear();
        }
    }
}