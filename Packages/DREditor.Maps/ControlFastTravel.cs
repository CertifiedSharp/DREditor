//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ControlFastTravel : FastTravelController<FastTravelData>
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFastTravel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedRoomTransitions = null;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            onFinishedRoomTransitions.Response.AddListener(SetFastTravelability);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            onFinishedRoomTransitions.Response.RemoveListener(SetFastTravelability);
        }

        /// <inheritdoc/>
        public override void Load(string data)
        {
            base.Load(data);

            if (this.data.SetOnAwake)
                SetFastTravelability();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetFastTravelability() => canFastTravel.SetValue(data.SetFastTravelTo);
    }
}