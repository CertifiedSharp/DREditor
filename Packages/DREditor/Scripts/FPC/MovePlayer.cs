//Author: Happy30
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.FPC
{
    [RequireComponent(typeof(CharacterController))]
    public class MovePlayer : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;

        public float WalkSpeed;
        public float RunSpeed;
        public float JumpSpeed;
        //private float _cameraHeight;

        //private float timer;
        public float Gravity = 9.89f;
        private float _downForce;

        private Vector3 _initialParentPos;

        CharacterController _characterController;

        public BoolWithEvent InDialogue;
        public BoolWithEvent Running;

        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        //bool toggleSprint = false;

        [Tooltip("")]
        [SerializeField] protected InputActionReference sprint;

        [Tooltip("")]
        [SerializeField] protected InputActionReference move;

        float hVelocity;
        float vVelocity;
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _initialParentPos = transform.parent.position;
            UnityEngine.Debug.Log("Awake Of Move Player Called");
            InDialogue.Value = false;
            sprint.action.started += cont => Sprint();
            sprint.action.canceled += cont => CancelSprint();
        }
        private void Start()
        {
            UnityEngine.Debug.Log("Start Of Move Player Called");
            SetMoveSpeed(WalkSpeed);
            Running.Value = false;
        }

        private void OnEnable()
        {
            SetMoveSpeed(WalkSpeed);
        }

        void Sprint()
        {
            //if (toggleSprint)
            //{
            //    MoveSpeed = MoveSpeed == WalkSpeed ? RunSpeed : WalkSpeed;
            //    return;
            //}
            SetMoveSpeed(RunSpeed);
            Running.Value = true;
        }
        void CancelSprint()
        {
            //if (toggleSprint)
            //    return;

            SetMoveSpeed(WalkSpeed);
            Running.Value = false;
        }

        protected virtual void SetMoveSpeed(float moveSpeed) => this.moveSpeed = moveSpeed;

        private void Update()
        {
            if (InDialogue.Value) return;

            //MoveSpeed = (Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed);
            //Running.Value = Input.GetKey(KeyCode.LeftShift);




#if ENABLE_INPUT_SYSTEM
            hVelocity = move.action.ReadValue<Vector2>().x;
            vVelocity = move.action.ReadValue<Vector2>().y;
#elif ENABLE_LEGACY_INPUT_MANAGER
			MoveSpeed = (Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed);
			Running.Value = Input.GetKey(KeyCode.LeftShift);
			hVelocity = Input.GetAxisRaw("Horizontal");
			vVelocity = Input.GetAxisRaw("Vertical");
			if (_characterController.isGrounded && Input.GetButton("Jump")) {
				_downForce = JumpSpeed;
			}
#endif

            var moveTowardsPos = new Vector3(hVelocity, 0, vVelocity);
            moveTowardsPos = transform.TransformDirection(moveTowardsPos);
            
            
            /*if (_characterController.isGrounded && Input.GetButton("Jump")) {
                _downForce = JumpSpeed;
            }*/

            if (!_characterController.isGrounded)
            {
                _downForce -= Gravity * Time.deltaTime;
            }
            else
            {
                _downForce = 0;  //FIX THIS STINKY CODE
            }


            moveTowardsPos = moveTowardsPos.normalized;
            moveTowardsPos = moveTowardsPos * moveSpeed * Time.deltaTime;

            if (transform.parent.position != _initialParentPos)
            {
                moveTowardsPos += transform.parent.position;
                transform.parent.position = _initialParentPos;
            }
                   

             _characterController.Move(new Vector3(moveTowardsPos.x, _downForce * Time.deltaTime, moveTowardsPos.z));
        }


        public virtual void SetPlayerPosition() // Called from Scene Event Listener OnSetPlayerState Scene Event
        {
            transform.position = playerSpawnPoint.Value.Pos;

            transform.localEulerAngles
                = new Vector2(transform.localEulerAngles.x, playerSpawnPoint.Value.Rot.eulerAngles.y);
        }
    }
}