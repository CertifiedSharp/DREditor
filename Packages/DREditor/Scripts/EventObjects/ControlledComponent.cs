//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ControlledComponent : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip("")]
        [SerializeField] protected bool debug = false;
#endif
        [Tooltip("")]
        [SerializeField] protected bool configureManually = false;

        [ShowIf(nameof(NotUsingManual))]
        [FormerlySerializedAs("caInputAllowed")]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool NotUsingManual() => !configureManually;

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableInput() => inputAllowed.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableInput() => inputAllowed.SetValue(false);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InputAllowed() => inputAllowed.Value;

        protected virtual void Awake()
        {
            if (NotUsingManual() && inputAllowed == null)
                UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}'s Controllable field {nameof(inputAllowed)} is missing!" +
                    $"\nPROPOSAL: Either assign the value or check the {nameof(configureManually)} option.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (inputAllowed != null)
                inputAllowed.AddListener(OnUpdateInput);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (inputAllowed != null)
                inputAllowed.RemoveListener(OnUpdateInput);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputAllowed"></param>
        protected virtual void OnUpdateInput(bool inputAllowed)
        {
            if (inputAllowed)
                ActivateControls();
            else
                DeactivateControls();
        }


        /// <summary>
        ///
        /// </summary>
        public abstract void ActivateControls();

        /// <summary>
        ///
        /// </summary>
        public abstract void DeactivateControls();


        protected virtual void Start()
        {
#if UNITY_EDITOR
            if (debug)
                ActivateControls();
#endif
        }
    }
}