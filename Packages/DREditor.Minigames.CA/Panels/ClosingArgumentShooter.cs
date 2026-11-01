// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling.Reticles;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(IClosingArgumentResetable) +
        "\n\n")]
    public class ClosingArgumentShooter : ControlledComponent, IClosingArgumentResetable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference choose = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent caPanelTarget = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent caResult = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected HoverReticle reticle = null;


        [Header("")]
        [SerializeField] protected UnityEvent OnStockFly = null;


        /// <summary>
        ///
        /// </summary>
        protected IActiveStockPanel activeStockPanel;

        /// <summary>
        ///
        /// </summary>
        protected IClosingArgumentListener[] argumentListeners;

        /// <summary>
        /// 
        /// </summary>
        protected QuestionPanelBase hoveredPanel;


        /// <inheritdoc/>
        public override void ActivateControls() 
            => choose.action.performed += EvaluateSelectedStock;

        /// <inheritdoc/>
        public override void DeactivateControls() 
            => choose.action.performed -= EvaluateSelectedStock;


        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void EvaluateSelectedStock(InputAction.CallbackContext context)
        {
            hoveredPanel = reticle.GetHoveredObject<QuestionPanelBase>();

            if (!(hoveredPanel != null && context.performed))
                return;

            activeStockPanel ??= transform.root.GetComponentInChildren<IActiveStockPanel>();

            if (!activeStockPanel.CurrentStockSelectable())
                return;

            argumentListeners ??= FindObjectsByType<MonoBehaviour>().OfType<IClosingArgumentListener>().ToArray();

            OnStockFly?.Invoke();

            DisableInput();

            StartCoroutine(ShootPanelRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShootPanelRoutine()
        {
            SetPanelTargetPosition();

            yield return ShootStockAnimation();

            ProcessPanelShot();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetPanelTargetPosition()
        {
            Transform hoveredPanelTransform = hoveredPanel.transform;

            caPanelTarget.SetValue(new(hoveredPanelTransform.position, Quaternion.identity));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShootStockAnimation() => activeStockPanel.ShootStock();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ProcessPanelShot()
        {
            bool result = ShotStockResult();

            if (caResult.Value != result)
                caResult.SetValue(result);
            else
                caResult.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShotStockResult()
        {
            Transform hoveredStockTransform = hoveredPanel.GetStockTransform();

            return activeStockPanel.CurrentStockMatches(hoveredStockTransform);
        }


        /// <inheritdoc/>
        public virtual void CAReset() => argumentListeners = null;
    }
}