//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [ClassHeader(nameof(IActuatorBehaviour) +
        "\n\n")]
    public class HitboxActuator : ActuatorBehaviourBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        protected HitboxActuatorData data = new();


        /// <summary>
        ///
        /// </summary>
        protected BoxCollider hitbox = null;


        /// <inheritdoc/>
        protected override void InitializeActuator()
        {
            hitbox = GetComponent<BoxCollider>();

            if (data.Triggered == false)
                hitbox.enabled = true;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!data.Triggered)
            {
                data.SetTriggered(!data.Triggered);

                hitbox.enabled = false;

                base.Call();
            }
        }

        /// <inheritdoc/>
        public override object Save()
        {
            HitboxActuatorData newData = (HitboxActuatorData)data.Clone();

            newData.SetType(GetType().Name);

            newData.SetTriggered(data.Triggered);

            newData.SetPosition(transform.position);

            newData.SetHitboxSize(hitbox.size);

            return newData;
        }

        /// <inheritdoc/>
        public override void Load(object ob)
        {
            data = (HitboxActuatorData)Convert.ChangeType(ob, typeof(HitboxActuatorData));

            transform.position = data.position;

            if (!TryGetComponent(out hitbox))
                hitbox = gameObject.AddComponent<BoxCollider>();

            hitbox.enabled = false;

            hitbox.isTrigger = true;

            hitbox.size = data.HitboxSize;
        }
    }
}