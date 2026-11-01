//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RegulationsUIBase : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Header("Required")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference navigate = null;


        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ScriptableObject regulationsDatabase = null;


        /// <summary>
        ///
        /// </summary>
        protected IRegulationsUIRepository repository = null;

        /// <summary>
        ///
        /// </summary>
        protected List<IRegulation> regulations = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            InitializeRepository();

            ResetUI();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeRepository()
        {
            repository = (IRegulationsUIRepository)regulationsDatabase;

            regulations = repository?.GetRegulations();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            menu.BeforeStart.AddListener(ResetUI);

            menu.StartEvents.AddListener(ActivateControls);

            menu.EndEvents.AddListener(DeactivateControls);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            menu.BeforeStart.RemoveListener(ResetUI);

            menu.StartEvents.RemoveListener(ActivateControls);

            menu.EndEvents.RemoveListener(DeactivateControls);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void ResetUI();


        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateControls() => navigate.action.performed += ChangeRegulation;

        /// <summary>
        ///
        /// </summary>
        protected virtual void DeactivateControls() => navigate.action.performed -= ChangeRegulation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected abstract void ChangeRegulation(CallbackContext context);
    }
}