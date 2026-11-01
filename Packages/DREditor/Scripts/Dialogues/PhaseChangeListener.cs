//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PhaseChangeListener<T> : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected List<IPhaseChangeEvent<T>> PhaseChangeEvents;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => PhaseChangeEvents = FindObjectsByType<MonoBehaviour>().OfType<IPhaseChangeEvent<T>>().ToList();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            foreach (var PhaseChangeEvent in PhaseChangeEvents)
                if (PhaseChangeEvent != null)
                    PhaseChangeEvent.PhaseChanged += OnPhaseChange;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach(var PhaseChangeEvent in PhaseChangeEvents)
                if (PhaseChangeEvent != null)
                    PhaseChangeEvent.PhaseChanged -= OnPhaseChange;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phaseData"></param>
        protected abstract void OnPhaseChange(T phaseData);
    }
}