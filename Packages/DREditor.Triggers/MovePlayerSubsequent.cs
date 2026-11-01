//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Dialogues.Events;
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISubsequentBehaviour) +
        "\n\n")]
    public class MovePlayerSubsequent : MonoBehaviour, ISubsequentBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform location = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool isStatic = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueCameraToMainEventString = "DialogueCameraToMainCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string playerGameObjectName = "Player";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";


        /// <summary>
        ///
        /// </summary>
        protected GameObject player;

        /// <summary>
        ///
        /// </summary>
        protected GameObject mainCamera;

        /// <summary>
        ///
        /// </summary>
        protected PositionEventSubsequentData data = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            player = GameObject.Find(playerGameObjectName);

            mainCamera = GameObject.FindWithTag(mainCameraTag);

            if (isStatic)
            {
                Vector3 position = location ? location.position : transform.position;

                Quaternion rotation = location ? location.rotation : transform.rotation;

                data.SetPositionRotation(position, rotation);
            }
        }

        /// <inheritdoc/>
        public virtual void Call()
        {
            UnityEngine.Debug.Log($"REPORT: Camera and Player moved to position: {data.position}");

            PnCCamera pncCamera = FindAnyObjectByType<PnCCamera>();

            pncCamera.enabled = false;

            SmoothMouseLook smoothMouseLook = pncCamera.gameObject.GetComponent<SmoothMouseLook>();
            
            smoothMouseLook.ClearCorePos();

            smoothMouseLook.SetCorePos(new Vector2(data.rotation.eulerAngles.y, data.rotation.eulerAngles.x), Vector2.zero);

            //FindAnyObjectByType<LookAtObjectOnSelection>().EnableSkipOGCameraLookIfInDialogue();

            player.transform.SetPositionAndRotation(data.position, data.rotation);

            FollowPlayer followPlayer = pncCamera.gameObject.GetComponent<FollowPlayer>();

            mainCamera.transform.SetPositionAndRotation(followPlayer.GetPlayerNoLerp(), data.rotation);//0.4008825

            //ServiceLocator.GetService<PlayerManager>().DialogueCamToMain();

            DialogueEventSystem.TriggerEvent(dialogueCameraToMainEventString);
        }

        /// <inheritdoc/>
        public virtual object Save()
        {
            PositionEventSubsequentData newData = (PositionEventSubsequentData)data.Clone();

            newData.SetType(GetType().Name);

            Vector3 position = location ? location.position : transform.position;

            Quaternion rotation = location ? location.rotation : transform.rotation;

            newData.SetPositionRotation(position, rotation);

            newData.SetEventString(dialogueCameraToMainEventString);

            return newData;
        }

        /// <inheritdoc/>
        public virtual void Load(object ob)
        {
            data = (PositionEventSubsequentData)Convert.ChangeType(ob, typeof(PositionEventSubsequentData));

            if (!isStatic)
                transform.SetPositionAndRotation(data.position, data.rotation);

            if (!string.IsNullOrEmpty(data.eventString))
                dialogueCameraToMainEventString = data.eventString;
        }
    }
}