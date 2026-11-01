//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(Slider))]
    [ClassHeader(nameof(OptionWithEventListener<float, FloatEvent>) +
        "\n\n")]
    public class SliderUIOption : OptionWithEventListener<float, FloatEvent>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onSliderSettingChanged = null;


        /// <summary>
        ///
        /// </summary>
        protected Slider slider;


        protected virtual void Awake() => slider = GetComponent<Slider>();

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            slider.value = valueWithEvent.Value;

            if (valueWithEvent != null)
                slider.onValueChanged.AddListener(OnSliderUpdated);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            if (valueWithEvent != null)
                slider.onValueChanged.RemoveListener(OnSliderUpdated);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newValue"></param>
        protected virtual void OnSliderUpdated(float newValue)
        {
            valueWithEvent.SetValue(newValue);

            if (valueWithEvent is SliderOption sliderOption && sliderOption.invokeTextOnSliderChanged)
                sliderOption.DisplayDescriptionEvent.Invoke();
        }

        /// <inheritdoc/>
        protected override void OnValueChanged(float updatedValue)
        {
            base.OnValueChanged(updatedValue);

            slider.onValueChanged.RemoveListener(OnSliderUpdated);

            slider.value = updatedValue;

            if (onSliderSettingChanged != null && slider.gameObject.activeInHierarchy && slider.enabled)
                onSliderSettingChanged.Raise();

            slider.onValueChanged.AddListener(OnSliderUpdated);
        }

        /// <inheritdoc/>
        public override void Initialize(IOptionEvent option)
        {
            base.Initialize(option);

            if (slider == null)
                slider = GetComponent<Slider>();

            if (option is SliderOption sliderOption)
            {
                slider.value = sliderOption.value;

                slider.minValue = sliderOption.min; 

                slider.maxValue = sliderOption.max;

                onSliderSettingChanged = sliderOption.onSliderSettingChanged;
            }
        }
    }
}