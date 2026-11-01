//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
using DREditor.Dialogues.Events;
#endif
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    public class PanToObjectEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera dialogueCamera = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected string dialogueEventStringCamAppend = "AppendToCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float characterPanTime = 0.3f;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            Transform body = (Transform)o;

            /* DOKill is here because during testing, PostTrialDialogue, Kai's : That's uh.. that's not
             * Loading to that line caused the camera to pan to tozu because there were so many calls to it
             * from load to line. Just noting this in case I forget.
             */
#if DOTWEEN
            dialogueCamera.transform.DOKill();
#endif
            if (inFastForwardMode.Value || isLoadingFile.Value)
                dialogueCamera.transform.SetPositionAndRotation(body.position, body.rotation);
            else
            {
#if DOTWEEN
                DialogueEventSystem.TriggerEvent(dialogueEventStringCamAppend, 
                    dialogueCamera.transform.DOMove(body.position, characterPanTime));

                dialogueCamera.transform.DORotateQuaternion(body.rotation, characterPanTime);
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("PanToObject");
    }
}