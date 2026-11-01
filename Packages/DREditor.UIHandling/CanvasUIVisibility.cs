//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class CanvasUIVisibility : UIVisibilityComponent, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Canvas canvas = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool setVisibilityOnTitleScreen = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool visibilityOnTitleScreen = false;


        protected virtual void Awake() => CheckInitializeCanvas();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CheckInitializeCanvas()
        {
            if (canvas == null)
                TryGetComponent(out canvas);

            if (canvas == null)
                canvas = transform.parent.GetComponentInParent<Canvas>();
        }


        /// <inheritdoc/>
        protected override void ShowUI() => SetCanvasTo(true);

        /// <inheritdoc/>
        protected override void HideUI() => SetCanvasTo(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        protected virtual void SetCanvasTo(bool to)
        {
            if (debug)
                UnityEngine.Debug.LogWarning($"REPORT: Set Canvas {gameObject.name} to {to}.");

            CheckInitializeCanvas();

            canvas.enabled = to;
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            yield break;
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            if (setVisibilityOnTitleScreen)
                ChangeVisibilityOnTitleScreen();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeVisibilityOnTitleScreen() => SetCanvasTo(visibilityOnTitleScreen);

        /// <summary>
        ///
        /// </summary>
        /// <param name="canvas"></param>
        /// <remarks></remarks>
        public void SetCanvas(Canvas canvas) => this.canvas = canvas;
    }
}