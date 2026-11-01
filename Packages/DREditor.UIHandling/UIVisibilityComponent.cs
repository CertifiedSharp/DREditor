//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class UIVisibilityComponent : MonoBehaviour, IUIVisibility, ITrackableVisibility
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool skipShowTillComplete = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool skipHideTillComplete = false;


        /// <summary>
        ///
        /// </summary>
        private bool visible = false;


        public virtual bool Visible { get => visible; }


        /// <inheritdoc/>
        public virtual void Show() => MakeUIVisibleState(true);

        /// <inheritdoc/>
        public virtual void Hide() => MakeUIVisibleState(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="newVisibleState"></param>
        protected virtual void MakeUIVisibleState(bool newVisibleState)
        {
            SetVisible(newVisibleState);

            InvokeVisibleState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="visible"></param>
        protected virtual void SetVisible(bool visible) => this.visible = visible;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeVisibleState()
        {
            if (Visible)
                ShowUI();
            else
                HideUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void ShowUI();

        /// <summary>
        ///
        /// </summary>
        protected abstract void HideUI();


        /// <inheritdoc/>
        public virtual IEnumerator WaitForChangeToComplete()
        {
            if (!SkipWaitingToComplete())
                yield return WaitForUIToComplete();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkipWaitingToComplete() => ShouldSkipWaitingForShow() || ShouldSkipWaitingForHide();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldSkipWaitingForShow() => Visible && skipShowTillComplete;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldSkipWaitingForHide() => !Visible && skipHideTillComplete;


        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator WaitForUIToComplete();


        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator ShowTillComplete()
        {
            Show();

            yield return WaitForChangeToComplete();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator HideTillComplete()
        {
            Hide();

            yield return WaitForChangeToComplete();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="show"></param>
        public virtual void Invoke(bool show)
        {
            if (show)
                Show();
            else
                Hide();
        }
    }
}