//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Additional Author Henry.
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class Reticle : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference cursor = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool lockedOnStart = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string keyboardMouseScheme = "Keyboard&Mouse";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float tpfdDistance = 20;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float reticleSelectAnimationDuration = 2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float keyboardSensitivity = 50;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float controllerSensitivity = 100;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float sensitivityDivisor = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool clampToScreen = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ValueWithEvent<float, FloatEvent> reticleSpeedSetting = null;


        /// <summary>
        ///
        /// </summary>
        protected bool locked = false;

        /// <summary>
        ///
        /// </summary>
        protected PlayerInput input;

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        public virtual void SetLock(bool to) => locked = to;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            Initialize();

            Cursor.visible = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            if (!input)
                input = (PlayerInput)FindAnyObjectByType(typeof(PlayerInput));

            if (!input)
                UnityEngine.Debug.LogError("ALERT: You must have a Player Input referenced so Raycast Reticle can work in 2.5D mode!");

            if (!reticleSpeedSetting)
                UnityEngine.Debug.LogError("ALERT: ReticleSpeedSetting is not assigned! Reticle movement disabled!");

            if (!input || !reticleSpeedSetting)
                enabled = false;

            if (lockedOnStart)
                SetLock(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Update() => Move();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Move()
        {
            if (locked)
                return;

            Vector3 calculatedMovement = CalculateMovement(GrabMovement());
            
            ApplyCalculatedMovement(calculatedMovement);

            if (clampToScreen)
                ClampToScreen();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual Vector3 GrabMovement()
        {
            Vector2 delta = cursor.action.ReadValue<Vector2>();

            return new Vector3(delta.x, delta.y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputMovement"></param>
        /// <returns></returns>
        protected virtual Vector3 CalculateMovement(Vector3 inputMovement)
        {
            if (reticleSpeedSetting.Value == 0)
                UnityEngine.Debug.LogError("ALERT: The reticle speed setting is zero! Reticle movement disabled!");

            float sensitivity = input.currentControlScheme == keyboardMouseScheme ? keyboardSensitivity : controllerSensitivity;

            return sensitivity * inputMovement / sensitivityDivisor * reticleSpeedSetting.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="calculatedMovement"></param>
        protected virtual void ApplyCalculatedMovement(Vector3 calculatedMovement) => transform.position += calculatedMovement;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClampToScreen()
        {
            if (transform.position.x > Screen.width)
                transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);
            else if (transform.position.x < 0)
                transform.position = new Vector3(0, transform.position.y, transform.position.z);

            if (transform.position.y > Screen.height)
                transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
            else if (transform.position.y < 0)
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}