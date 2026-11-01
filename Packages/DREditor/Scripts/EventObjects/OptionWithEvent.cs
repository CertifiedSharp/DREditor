//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public abstract class OptionWithEvent<T, TY> : ValueWithEvent<T, TY>, IOptionEvent where TY : UnityEvent<T>, new()
    {
        [Tooltip("")]
        [SerializeField] protected string optionDisplayName = "";

        [Tooltip("")]
        [SerializeField] protected StringWithEvent displayDescriptionEvent;

        [TextArea]
        [Tooltip("")]
        [SerializeField] protected string optionDescription = "";


        public string OptionDisplayName { get => optionDisplayName; }

        public StringWithEvent DisplayDescriptionEvent { get => displayDescriptionEvent; }

        public string OptionDescription { get => optionDescription; }
    }
}
