//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.Cameras
{
    
    [RequireComponent(typeof(CharacterController))]
    public class Crouch : MonoBehaviour
    {
        private CharacterController _characterController;
        [SerializeField] bool disable = false;
        private float targetHeight;

        public float CrouchHeight;
        float NormalHeight;
        public float CrouchOmega;

        public bool ToggleToCrouch;
        [Tooltip("")]
        [SerializeField] protected InputActionReference crouch;
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            NormalHeight = _characterController.height;
            targetHeight = NormalHeight;
        }

        private void Start()
        {
            if (disable)
                return;
#if ENABLE_INPUT_SYSTEM
            crouch.action.started += cont => CrouchCall();
            crouch.action.canceled += cont => UnCrouchCall();
#endif
        }
        void CrouchCall()
        {
            if (ToggleToCrouch)
            {
                targetHeight = targetHeight == NormalHeight ? CrouchHeight : NormalHeight;
                return;
            }
            targetHeight = CrouchHeight;
        }
        void UnCrouchCall()
        {
            if (ToggleToCrouch)
                return;
            targetHeight = NormalHeight;
        }

        void Update()
        {
            
            _characterController.height = Mathf.Lerp(_characterController.height, targetHeight, CrouchOmega * Time.deltaTime);

            //ETween.Step(_characterController.height, targetHeight, CrouchOmega);
        }
    }
}