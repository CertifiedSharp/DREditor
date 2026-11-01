//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DREditor.UI.ReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuModifier) +
        "\n\n")]
    public class ReportCardMenu : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ReportCardControllableComponent panel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("characters")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField]
        protected List<string> names = new()
        {
            "Damon", "Diana", "Jean", "Jett", "Toshiko", "Mark", "Ulysses", "Wolfgang",
            "Cassidy", "Desmond", "Eloise", "Kai", "Grace", "Ingrid", "Wenona", "Eva"
        };


        [Header("")]
        [SerializeField] protected UnityEvent OnUINavigated = null;


        /// <summary>
        ///
        /// </summary>
        protected bool controlsActive = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (panel != null)
                panel.SetCharacterNames(names);
            else
                UnityEngine.Debug.LogWarning(
                    "ALERT: The panel for the Report Card Menu has not been assigned and therefore will not work." +
                    "\nPROPOSAL: Assign a " + nameof(ReportCardControllableComponent) + " to the panel reference on the report card script.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (panel != null)
                menu.BeforeStart.AddListener(panel.ResetComponent);

            menu.StartEvents.AddListener(ActivateControls);

            menu.EndEvents.AddListener(DeactivateControls);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (panel != null)
                menu.BeforeStart.RemoveListener(panel.ResetComponent);

            menu.StartEvents.RemoveListener(ActivateControls);

            menu.EndEvents.RemoveListener(DeactivateControls);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateControls()
        {
            if (controlsActive)
            {
                UnityEngine.Debug.LogWarning("ALERT: REPORT CARD CONTROLS WERE ALREADY ACTIVATED!");

                return;
            }

            controlsActive = true;

            if (panel != null)
            {
                panel.ActivateControls();

                panel.SuccessfulUINavigation += OnUINavigated.Invoke;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DeactivateControls()
        {
            if (!controlsActive)
                return;

            controlsActive = false;

            if (panel != null)
            {
                panel.DeactivateControls();

                panel.SuccessfulUINavigation -= OnUINavigated.Invoke;
            }
        }


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Initialize Character Names")]
        public void InitializeCharacterNames()
        {
            if (characterDatabase != null)
            {
                names.Clear();

                var fteCharacters = characterDatabase.GetFTECharacterList();

                foreach (var character in fteCharacters)
                    names.Add(character.FirstName);
            }
            else
                UnityEngine.Debug.LogWarning("ALERT: You must assign the character database field to use this functionality!");
        }
    }
}