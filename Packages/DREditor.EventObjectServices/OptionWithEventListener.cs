//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public abstract class OptionWithEventListener<TX, TY> : ValueWithEventListener<OptionWithEvent<TX, TY>, TX, TY>, IOptionEventListener,
        ISceneEventListener
        where TY : UnityEvent<TX>, new()
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent newGameSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text displayNameText;


        public virtual void Initialize(IOptionEvent option)
        {
            if (displayNameText != null && displayNameText.gameObject.activeSelf)
                displayNameText.text = option.OptionDisplayName;

            valueWithEvent = option as OptionWithEvent<TX, TY>;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (newGameSelected != null)
                newGameSelected.RegisterListener(this);
        }

        protected override void OnDisable()
        {
            base.OnEnable();

            if (newGameSelected != null)
                newGameSelected.UnregisterListener(this);
        }

        public virtual void OnEventRaised() => valueWithEvent.SetValue(valueWithEvent.InitialValue);
    }
}