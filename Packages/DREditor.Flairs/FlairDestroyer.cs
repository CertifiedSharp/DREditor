//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FlairDestroyer : MonoBehaviour, IFlairDestroyable
    {
        [Header("")]
        [SerializeField] protected UnityEvent<IFlairDestroyable> onFlairDestroyed = null;


        public virtual UnityEvent<IFlairDestroyable> OnFlairDestroyed => onFlairDestroyed;


        /// <summary>
        ///
        /// </summary>
        public virtual void DestroyFlair() => Destroy(gameObject);


        protected virtual void OnDestroy() => OnFlairDestroyed?.Invoke(this);
    }
}