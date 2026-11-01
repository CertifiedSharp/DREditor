//Author: Happy30
using DREditor.Processables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Events/Scene Event", fileName = "New Scene Event")]
    public class SceneEvent : ScriptableObject
    {
        [Tooltip("")]
        [SerializeField] protected bool debug = false;


        /// <summary>
        ///
        /// </summary>
        private readonly List<ISceneEventListener> _listeners = new();


        public virtual void Raise()
        {
            DebugLogReport();

            RaiseAllListenersIn(GetOrderedListenersByPriority());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DebugLogReport()
        {
            if (debug)
                UnityEngine.Debug.Log($"REPORT: Scene Event {name} has been invoked.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<ISceneEventListener> GetOrderedListenersByPriority()
        {
            List<ISceneEventListener> ordered = _listeners
                .AsEnumerable()
                .Reverse()
                .ToList();

            bool hasPriority = ordered.Any(listener => listener is IPrioritizable prioritizable && prioritizable.Priority != 0);

            if (hasPriority)
                ordered = ordered
                    .OrderBy(listener => listener is IPrioritizable ? 0 : 1)
                    .ThenBy(listener => listener is IPrioritizable prioritizable ? prioritizable.Priority : 0)
                    .ToList();

            return ordered;
        }


        protected virtual void RaiseAllListenersIn(List<ISceneEventListener> ordered)
        {
            for (var i = 0; i < ordered.Count; i++)
                ordered[i].OnEventRaised();
        }


        public virtual void RegisterListener(ISceneEventListener listener)
        {
            ReportRegistry(listener, true);

            _listeners.Add(listener);
        }

        public virtual void UnregisterListener(ISceneEventListener listener)
        {
            ReportRegistry(listener, false);

            _listeners.Remove(listener);
        }


        protected virtual void ReportRegistry(ISceneEventListener listener, bool registering)
        {
            string registrationState = registering ? "Registers" : "Unregisters";

            if (debug)
                UnityEngine.Debug.Log($"REPORT: Scene Event {name} {registrationState}: " + listener.ToString());
        }
    }
}