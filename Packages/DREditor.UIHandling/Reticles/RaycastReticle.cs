//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.FPC;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(Reticle) + ", " + nameof(HoverReticle) +
        "\n\n")]
    public class RaycastReticle : HoverReticle
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference fire = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LayerMask selectableLayers = 1 << 6 | 1 << 11 | 1 << 15;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LayerMask blockedLayers = 1 << 17;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float threeDDistance = 1.5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent reticleCanSelect = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition inSafeTPFDState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition allowRaycasting = null;


        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera targetCamera;

        /// <summary>
        ///
        /// </summary>
        protected Ray ray;

        /// <summary>
        ///
        /// </summary>
        protected float distance;

        /// <summary>
        ///
        /// </summary>
        protected RaycastHit hit;

        /// <summary>
        ///
        /// </summary>
        protected bool temporarilyDisabled = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => reticleCanSelect.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => reticleCanSelect.SetValue(false);


        /// <summary>
        ///
        /// </summary>
        protected override void Start()
        {
            base.Start();

            var behaviourControllers = FindObjectsByType<ControlMonobehaviours>().ToList();

            foreach (var controller in behaviourControllers)
            {
                if (controller.RaycastScript == null)
                {
                    controller.SetRaycastScript(this);

                    controller.MonoBehaviours.Add(this);
                }
            }

            if (targetCamera == null)
                targetCamera = UnityEngine.Camera.main;
        }


        /// <summary>
        ///
        /// </summary>
        protected override void Move()
        {
            if (!temporarilyDisabled && allowRaycasting.Evaluate())
            {
                SetRayDistanceCursor();

                EvaluateHover();
            }

            if (!inTPFD.Value)
                transform.localPosition = Vector3.zero;
            else if (input && inSafeTPFDState.Evaluate())
                base.Move();
            else if (!inDialogue.Value)
                transform.localPosition = Vector3.zero;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetRayDistanceCursor()
        {
            if (targetCamera == null)
                targetCamera = UnityEngine.Camera.main;

            if (targetCamera == null)
                return;

            if (inSafeTPFDState.Evaluate())
                SetTPFDRay();
            else
                SetThreeDRay();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetTPFDRay()
        {
            ray = targetCamera.ScreenPointToRay(transform.position);

            SetCursorPlusDistance(tpfdDistance, CursorLockMode.Confined);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetThreeDRay()
        {
            ray = new Ray(targetCamera.transform.position, targetCamera.transform.TransformDirection(Vector3.forward));

            SetCursorPlusDistance(threeDDistance, CursorLockMode.Locked);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="lockMode"></param>
        protected virtual void SetCursorPlusDistance(float distance, CursorLockMode lockMode)
        {
            this.distance = distance;

            Cursor.lockState = lockMode;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateHover()
        {
            bool hasHit = Physics.Raycast(ray, out hit, distance, blockedLayers | selectableLayers);

            bool isHoveringObject = hasHit && (selectableLayers.value & (1 << hit.collider.gameObject.layer)) != 0;

            if (isHoveringObject)
                HoverObject(hit, hit.transform);
            else
                StopHoveringObject();
        }
        

        /// <inheritdoc/>
        protected override void HoverObject(RaycastHit hit, object hoveredObject)
        {
            base.HoverObject(hit, hoveredObject);

            EvaluateObjectClick();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateObjectClick()
        {
            if (fire.action.triggered)
            {
                OnConfirmSelection?.Invoke();

                hit.transform.gameObject.GetComponent<ISelectableObject>().SelectObject();

                reticleComponents.ForEach(component => component.OnSelected(hit.transform));

                StartCoroutine(TemporarilyDisable());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual IEnumerator TemporarilyDisable()
        {
            temporarilyDisabled = true;

            yield return new WaitUntil(() => !allowRaycasting.Evaluate());

            temporarilyDisabled = false;
        }
    }
}