//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues.Events;
using UnityEngine;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ShakeUIEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform shakenUI = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
#if DOTWEEN
            ShakeObject data = (ShakeObject)o;

            shakenUI.DOShakeAnchorPos(data.duration, data.strength, data.vibrato, data.randomness, data.snapping, data.fadeOut);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ShakeCG");

        /// <summary>
        /// 
        /// </summary>
        [BehaviourButton]
        public void TestShake()
        {
            if (Application.isPlaying)
                DialogueEventSystem.TriggerEvent(dialogueEventString, new ShakeObject());
            else
                UnityEngine.Debug.LogWarning($"{ToString()}: Game must be in play mode!");
        }
    }
}