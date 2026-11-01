//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TrackedIndexListener : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected ITrackableIndex selector;


        protected virtual void Start()
        {
            if (InitializedSelector())
                SubscribeToIndexUpdated();
            else
                OnInitializationFailed();
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool InitializedSelector();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInitializationFailed() => UnityEngine.Debug.LogError("ALERT: Process failed to assign the selector!");

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToIndexUpdated() => selector.MovedIndexFrom += OnIndexUpdated;


        protected virtual void OnDestroy()
        {
            if (CachedSelectorValid())
                UnsubscribeToIndexUpdated();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CachedSelectorValid() => selector != null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToIndexUpdated() => selector.MovedIndexFrom -= OnIndexUpdated;

        /// <summary>
        ///
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        protected abstract void OnIndexUpdated(int from, int to);
    }
}