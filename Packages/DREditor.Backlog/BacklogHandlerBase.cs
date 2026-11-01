//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Backlogging
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BacklogHandlerBase : MonoBehaviour, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent startedNewGame = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            RegisterThisService();

            startedNewGame.RegisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            UnregisterThisService();

            startedNewGame.UnregisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RegisterThisService()
        {
            if (!ServiceLocator.HasService<BacklogHandlerBase>())
                ServiceLocator.Register(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnregisterThisService()
        {
            if (ServiceLocator.GetService<BacklogHandlerBase>() == this)
                ServiceLocator.Unregister(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract object[] GetBacklogLines();

        /// <summary>
        ///
        /// </summary>
        public abstract void ClearLines();

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public abstract void AddLine(object o = null);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract object SaveBacklog();

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public abstract void LoadBacklog(object o = null);


        /// <inheritdoc/>
        public virtual void OnEventRaised() => ClearLines();
    }
}