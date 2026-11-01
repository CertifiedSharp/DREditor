//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.States
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UpdateOnStateChange<T> : IntWithEventListener, IStateList
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Header("")]

        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected StateDatabase stateDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected List<StateChangeGroup<T>> groups = new();

        [Space]


        /// <summary>
        ///
        /// </summary>
        protected StateAsset activeState = null;

        /// <summary>
        ///
        /// </summary>
        protected int activeStateIndex = -1;

        /// <summary>
        ///
        /// </summary>
        protected T activeValue;

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<StateAsset, T> dictionaryStates;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => dictionaryStates = groups.ToDictionary(kv => kv.state, kv => kv.stateValue);

        protected override void OnEnable()
        {
            base.OnEnable();

            OnValueChanged(valueWithEvent.Value);
        }

        public virtual void OnInitializeStateList()
        {
            UnityEngine.Debug.Log("Initializing State change list.");

            if (stateDatabase == null)
                UnityEngine.Debug.LogWarning($"{ToString()}: State Database must be filled to populate inspector.");
            else
                foreach (var state in stateDatabase.States)
                    if (groups.Where(group => group.state == state).Count() == 0)
                        groups.Add(new StateChangeGroup<T>(state));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stateIndex"></param>
        protected override void OnValueChanged(int stateIndex)
        {
            base.OnValueChanged(stateIndex);

            activeStateIndex = stateIndex;

            InitializeStateChanged();

            OnStateChanged();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeStateChanged()
        {
            activeState = stateDatabase.States[activeStateIndex];

            if (dictionaryStates.TryGetValue(activeState, out var value))
                activeValue = value;
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnStateChanged();
    }
}