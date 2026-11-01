//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Input;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MenuGroupBase : MonoBehaviour, IUIVisibility, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugShow = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool isActive = false;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Header("")]
        public UnityEvent BeforeStart = null;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        public UnityEvent StartEvents = null;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        public UnityEvent EndEvents = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase backGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool hasBackInput = false;


        [Header("")]
        public UnityEvent OnBack = null;


        /// <summary>
        ///
        /// </summary>
        protected bool Changing { get; private set; }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (!ServiceLocator.HasService<MenuState>())
                ServiceLocator.Register(new MenuState());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => ServiceLocator.GetService<MenuState>().OnMenuStateChanged += OnMenuStateChanged;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (ServiceLocator.HasService<MenuState>())
                ServiceLocator.GetService<MenuState>().OnMenuStateChanged -= OnMenuStateChanged;

            if (ServiceLocator.HasService<IInputManager>())
                ServiceLocator.GetService<IInputManager>().GetCancel().started -= BackGroup;
        }

        protected virtual void OnDestroy()
        {
            if (ServiceLocator.HasService<MenuState>())
                ServiceLocator.GetService<MenuState>().OnMenuStateChanged -= OnMenuStateChanged;

            if (ServiceLocator.HasService<IInputManager>())
                ServiceLocator.GetService<IInputManager>().GetCancel().started -= BackGroup;
        }

        protected virtual void Start()
        {
            if (debugShow)
                DebugShow();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Changing"></param>
        private void OnMenuStateChanged(bool Changing) => this.Changing = Changing;


        /// <summary>
        ///
        /// </summary>
        /// <param name="group"></param>
        public abstract void ChangeGroup(MenuGroupBase group);

        /// <summary>
        ///
        /// </summary>
        protected abstract void ResetGroup();

        /// <summary>
        ///
        /// </summary>
        public abstract void SetEvaluateSelect();

        /// <summary>
        ///
        /// </summary>
        public abstract void Show();

        /// <summary>
        ///
        /// </summary>
        public abstract void Hide();

        /// <inheritdoc/>
        public virtual IEnumerator WaitForChangeToComplete()
        {
            yield return new WaitUntil(() => !Changing);

            yield break;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        protected virtual void SetActive(bool active) => isActive = active;

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public virtual void BackGroup(InputAction.CallbackContext context)
        {
            UnityEngine.Debug.Log("REPORT: Calling BackGroup on : " + gameObject.name);

            if (backGroup != null)
            {
                ChangeGroup(backGroup);

                ServiceLocator.GetService<SFXManagerBase>().PlayCancel();
            }

            OnBack?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void AddBackInput() => StartCoroutine(SetBackInput(true));

        /// <summary>
        ///
        /// </summary>
        public virtual void RemoveBackInput() => StartCoroutine(SetBackInput(false));

        /// <summary>
        ///
        /// </summary>
        /// <param name="setBackInput"></param>
        protected virtual IEnumerator SetBackInput(bool setBackInput)
        {
            if (hasBackInput == setBackInput)
                yield break;

            yield return null;

            hasBackInput = setBackInput;

            if (hasBackInput)
                ServiceLocator.GetService<IInputManager>().GetCancel().started += BackGroup;
            else
                ServiceLocator.GetService<IInputManager>().GetCancel().started -= BackGroup;
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            RemoveBackInput();

            ServiceLocator.GetService<MenuState>().SetSelect(false);

            ResetGroup();

            ServiceLocator.GetService<MenuState>().SetSelect(true);
        }


        /// <inheritdoc/>
        [ContextMenu("Show")]
        public void DebugShow() => Show();
    }
}