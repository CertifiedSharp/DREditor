//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIBehaviour) + ", " + nameof(ISelectHandler) + ", " + nameof(IOptionEventListener) +
        "\n\n")]
    public class SelectStringWithEvent : UIBehaviour, ISelectHandler, IOptionEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent stringWithEvent = null;

        /// <summary>
        ///
        /// </summary>
        [TextArea]
        [Tooltip("")]
        [SerializeField] protected string textForEvent;

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnSelect(BaseEventData eventData) => stringWithEvent.SetValue(textForEvent);


        /// <inheritdoc/>
        public virtual void Initialize(IOptionEvent option)
        {
            stringWithEvent = option.DisplayDescriptionEvent;

            textForEvent = option.OptionDescription;
        }
    }
}