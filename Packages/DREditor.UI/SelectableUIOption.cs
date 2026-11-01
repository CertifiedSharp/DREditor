//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIBehaviour) + ", " + nameof(ISelectHandler) + ", " + nameof(IDeselectHandler) +
        "\n\n")]
    public class SelectableUIOption : UIBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("")]
        public UnityEvent Selection;

        [Header("")]
        public UnityEvent Deselection;


        /// <summary>
        ///
        /// </summary>
        protected SelectHandler[] handlers;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            handlers = GetComponents<SelectHandler>();
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var handler in handlers)
                Selection.AddListener(handler.Select);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (var handler in handlers)
                Selection.RemoveListener(handler.Select);
        }

        /// <summary>
        ///
        /// </summary>
        public void InvokeSelect() => Selection?.Invoke();

        /// <summary>
        ///
        /// </summary>
        public void InvokeDeselect() => Deselection?.Invoke();


        /// <summary>
        ///
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnSelect(BaseEventData eventData) => Selection?.Invoke();

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDeselect(BaseEventData eventData) => Deselection?.Invoke();
    }
}