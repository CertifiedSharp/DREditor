//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing.Handling;
using DREditor.Services;
using System;
using System.Linq;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IActuatorBehaviour) +
        "\n\n")]
    public class OLFlagActuator : ActuatorBehaviourBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FlagActuatorData data = new();


        /// <inheritdoc/>
        protected override void InitializeActuator()
        {
            if (data.Triggered == false)
            {
                var flags = data.flags.Where(flag => 
                ServiceLocator.GetService<FlagTriggerHandlerBase>().IsFlagTriggered(flag) == data.callIfFlagsAre);

                if (flags.Count() == data.flags.Count)
                {
                    data.SetTriggered(true);

                    base.Call();
                }
            }
        }

        /// <inheritdoc/>
        public override object Save()
        {
            FlagActuatorData newData = (FlagActuatorData)data.Clone();

            newData.SetType(GetType().Name);

            newData.SetTriggered(data.Triggered);

            newData.SetPosition(transform.position);

            newData.SetStringList(data.flags);

            newData.SetBoolValue(data.callIfFlagsAre);

            return newData;
        }

        /// <inheritdoc/>
        public override void Load(object ob)
        {
            data = (FlagActuatorData)Convert.ChangeType(ob, typeof(FlagActuatorData));

            transform.position = data.position;
        }
    }
}