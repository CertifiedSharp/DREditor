//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Original Author: Redberd36
//Sorry Red I ended up completely reworking this.
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class SpriteLookAtTarget : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inThreeDMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool mimicOG = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool ignoreReaderCheck = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";


        /// <summary>
        ///
        /// </summary>
        protected bool shouldLookAtTarget;

        /// <summary>
        ///
        /// </summary>
        protected Transform target;

        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager dialogueManager;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => inTPFD.AddListener(UpdateLookingStatus);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => inTPFD.RemoveListener(UpdateLookingStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inTPFD"></param>
        protected virtual void UpdateLookingStatus(bool inTPFD) => shouldLookAtTarget = !inTPFD;

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            target = GameObject.FindWithTag(mainCameraTag).GetComponent<Transform>();

            if (inThreeDMode.Value)
                shouldLookAtTarget = true;

            if (!ignoreReaderCheck)
                dialogueManager = ServiceLocator.GetService<IDialogueManager>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FixedUpdate() => BillboardCheck();

        /// <summary>
        ///
        /// </summary>
        protected virtual void BillboardCheck()
        {
            if (shouldLookAtTarget && !isLoadingFile.Value && !DialogueIsPlaying())
                Billboard();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Billboard()
        {
            if (mimicOG)
                MimicBillboard();
            else
                LookAtBillboard();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DialogueIsPlaying() => ignoreReaderCheck || dialogueManager == null || inDialogue.Value;

        /// <summary>
        ///
        /// </summary>
        protected virtual void MimicBillboard()
        {
            Vector3 direction = target.transform.forward * -1;

            SetRotation(direction);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LookAtBillboard()
        {
            Vector3 direction = target.transform.position - transform.position;

            SetRotation(direction);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void SetRotation(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}