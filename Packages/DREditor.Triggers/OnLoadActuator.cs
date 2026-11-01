//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ActuatorBehaviourBase) + ", " + nameof(IActuatorBehaviour) + 
        "\n\n")]
    public class OnLoadActuator : ActuatorBehaviourBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SimpleActuatorData data = new();


        /// <inheritdoc/>
        protected override void InitializeActuator()
        {
            if (data.Triggered == false)
            {
                UnityEngine.Debug.Log("REPORT: On Load Actuator is being Triggered");

                data.SetTriggered(true);

                base.Call();
            }
        }

        /// <inheritdoc/>
        public override object Save()
        {
            SimpleActuatorData newData = (SimpleActuatorData)data.Clone();

            newData.SetType(GetType().Name);

            newData.SetTriggered(data.Triggered);

            newData.SetPosition(transform.position);

            return newData;
        }

        /// <inheritdoc/>
        public override void Load(object ob)
        {
            try
            {
                data = (SimpleActuatorData)Convert.ChangeType(ob, typeof(SimpleActuatorData));
            }
            catch (InvalidCastException ex)
            {
                UnityEngine.Debug.LogError($"ALERT: Failed {nameof(OnLoadActuator)} Object was of type {ob.GetType()} and not " +
                    $"{nameof(SimpleActuatorData)}!");

                UnityEngine.Debug.LogException(ex);
            }

            transform.position = data.position;
        }
    }
}