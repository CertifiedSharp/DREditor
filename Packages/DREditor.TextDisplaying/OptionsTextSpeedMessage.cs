//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(OptionsTextMessage) +
        "\n\n")]
    public class OptionsTextSpeedMessage : OptionsTextMessage
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextDisplayerBase displayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent[] clearOnMessagesChanged;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var message in clearOnMessagesChanged)
                message.AddListener(OnOtherMessagesChanged);
        }


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (var message in clearOnMessagesChanged)
                message.RemoveListener(OnOtherMessagesChanged);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public override void PlayMessage(string message)
        {
            ClearBox();

            displayer.SetWaitingForClick(true);

            displayer.DisplayText(box, new Dialogues.Line(message));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="updatedMessage"></param>
        protected virtual void OnOtherMessagesChanged(string updatedMessage)
        {
            displayer.StopAllCoroutines();

            displayer.ResolveDisplayingText(displayer.GetText());
        }

        /// <summary>
        ///
        /// </summary>
        protected override void ClearBox()
        {
            displayer.StopAllCoroutines();

            displayer.ResolveDisplayingText("");

            base.ClearBox();
        }
    }
}