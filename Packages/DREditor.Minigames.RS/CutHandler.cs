//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(ControlledComponent) +
        "\n\n")]
    public class CutHandler : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference swipe = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool outputDebugSwipeLogs = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected EvaluateBoolCondition preventCutsCondition;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int frameWindowForInput = 5;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float timeForSwordToCut = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float cutCooldown = 0.1f;


        [Header("")]
        [SerializeField] protected UnityEvent<Vector3, Vector3> OnSwiped;

        [Header("")]
        [SerializeField] protected UnityEvent OnHit;
        
        [Header("")]
        [SerializeField] protected UnityEvent OnMissed;


        /// <summary>
        ///
        /// </summary>
        protected readonly List<Vector2> inputs = new();

        /// <summary>
        ///
        /// </summary>
        protected ISword cutSword;

        /// <summary>
        ///
        /// </summary>
        protected ISwipeListener[] swipeListeners;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            swipeListeners = transform.root.GetComponentsInChildren<ISwipeListener>();

            cutSword = GetComponentsInChildren<ISword>()?.First();
            
            if (cutSword == null)
                UnityEngine.Debug.LogError($"ALERT: No {nameof(ISword)} found in children of {name}");
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            cutSword?.StopDetectingCuts();
            
            foreach (var listener in swipeListeners)
                if (listener != null)
                    OnSwiped.AddListener(listener.OnSwiped);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            DeactivateControls();

            foreach (var listener in swipeListeners)
                if (listener != null)
                    OnSwiped.RemoveListener(listener.OnSwiped);
        }

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            inputs.Clear();

            if (outputDebugSwipeLogs)
                UnityEngine.Debug.LogWarning($"REPORT: Enabling Controls");

            swipe.action.performed += Swipe;
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            StopAllCoroutines();

            cutSword?.StopDetectingCuts();

            if (outputDebugSwipeLogs)
                UnityEngine.Debug.LogWarning($"REPORT: Disabling Controls");

            swipe.action.performed -= Swipe;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Swipe(CallbackContext context)
        {
            if (preventCutsCondition != null && preventCutsCondition.Evaluate())
                return;

            Vector2 direction = context.ReadValue<Vector2>();

            if (!InputDirectionValid(direction))
                return;

            if (inputs.Count == 0)
                StartCoroutine(BufferSwipe());

            inputs.Add(direction);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected virtual bool InputDirectionValid(Vector2 direction)
        {
            bool directionEmpty = direction == Vector2.zero;

            bool roundedDirectionEmpty = Mathf.Round(direction.x) == 0 && Mathf.Round(direction.y) == 0;

            bool directionValid = !directionEmpty && !roundedDirectionEmpty;

            if (directionEmpty && outputDebugSwipeLogs)
                UnityEngine.Debug.Log($"REPORT: Attempted Cut Direction was Zero");

            if (roundedDirectionEmpty && outputDebugSwipeLogs)
                UnityEngine.Debug.Log($"REPORT: Swipe Input did not reach the threshold: " + direction);

            if (!directionValid)
                StartCoroutine(Cooldown());

            return directionValid;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferSwipe()
        {
            yield return new WaitForEndOfFrame();

            yield return new WaitForSecondsRealtime(Time.deltaTime * frameWindowForInput);
            
            DeactivateControls();

            SwipeAtDirection(CombineInputDirections());

            StartCoroutine(SwipeProcess());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Vector2 CombineInputDirections()
        {
            Vector2 combine = Vector2.zero;

            foreach (var i in inputs)
            {
                combine.x += Mathf.Round(i.x);

                combine.y += Mathf.Round(i.y);

                if (outputDebugSwipeLogs)
                    UnityEngine.Debug.Log($"REPORT: Input Filtered: " + i);
            }

            combine.x = Mathf.Clamp(combine.x, -1, 1);

            combine.y = Mathf.Clamp(combine.y, -1, 1);

            inputs.Clear();

            return combine;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void SwipeAtDirection(Vector2 direction)
        {
            if (outputDebugSwipeLogs)
                UnityEngine.Debug.Log($"REPORT: Verified Swipe Direction is: " + direction);

            OnSwiped?.Invoke(cutSword.AngleSword(direction), direction);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SwipeProcess()
        {
            cutSword?.StartDetectingCuts();

            yield return new WaitForSeconds(timeForSwordToCut);

            cutSword?.StopDetectingCuts();

            if (cutSword.RecentlyLandedHit)
                OnHit?.Invoke();
            else
                OnMissed?.Invoke();

            yield return Cooldown();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator Cooldown()
        {
            if (outputDebugSwipeLogs)
                UnityEngine.Debug.Log($"REPORT: Started cooldown");

            yield return new WaitForSeconds(cutCooldown);

            if (outputDebugSwipeLogs)
                UnityEngine.Debug.Log($"REPORT: Ended Cooldown");

            ActivateControls();
        }
    }
}