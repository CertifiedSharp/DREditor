//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(OptionUIGroup) +
        "\n\n")]
    public class DifficultyGroup : OptionUIGroup
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool startingGroup = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool loadLastOption = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent difficultyIndex = null;


        /// <summary>
        ///
        /// </summary>
        protected int difficulty = 0;

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<SelectableOption, UnityAction> onOptionSelections = new();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<SelectableOption, UnityAction> onOptionDeselections = new();


        protected virtual void OnEnable() => difficultyIndex.AddListener(OnDifficultyUpdated);

        protected virtual void OnDisable() => difficultyIndex.RemoveListener(OnDifficultyUpdated);

        protected virtual void Start() => OnDifficultyUpdated(difficultyIndex.Value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="newDifficulty"></param>
        protected virtual void OnDifficultyUpdated(int newDifficulty)
        {
            difficulty = newDifficulty;

            if (!isSetup)
                foreach (var option in options)
                    if (option is SelectableOption selectable)
                        selectable.KeepSelected(false);

            SetLastOption(options[difficulty]);
        }


        /// <inheritdoc/>
        public override void SetupGroup()
        {
            if (lastOption == null)
                InitializeLastOption();

            if (startingGroup)
                StartGroup();

            base.SetupGroup();

            SubscribeToDifficultyOptions();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToDifficultyOptions()
        {
            foreach (SelectableOption option in options.Cast<SelectableOption>())
            {
                void SetGroup() => SetGroupDifficulty(option);

                void ChangedDiff() => ChangedDiffcultyGroup(option);

                option.OnSelection.AddListener(SetGroup);

                option.OnDeselection.AddListener(ChangedDiff);

                onOptionSelections[option] = SetGroup;

                onOptionDeselections[option] = ChangedDiff;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        public virtual void SetGroupDifficulty(Selectable selectable)
        {
            difficulty = options.IndexOf(selectable);

            SetGameDifficulty();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="option"></param>
        public virtual void ChangedDiffcultyGroup(Selectable option) => ((SelectableOption)option).KeepSelected(!CheckSelectionInGroup());

        /// <inheritdoc/>
        public override void UnSetupGroup()
        {
            base.UnSetupGroup();

            ((SelectableOption)lastOption).KeepSelected(true);

            SetGameDifficulty();

            UnsubscribeToDifficultyOptions();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SetGameDifficulty() => difficultyIndex.SetValue(difficulty);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToDifficultyOptions()
        {
            foreach (SelectableOption option in options.Cast<SelectableOption>())
            {
                option.OnSelection.RemoveListener(onOptionSelections[option]);

                option.OnDeselection.RemoveListener(onOptionDeselections[option]);

                onOptionSelections.Remove(option);

                onOptionDeselections.Remove(option);
            }
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void StartGroup()
        {
            GameObject select = lastOption != null ? lastOption.gameObject : options[0].gameObject;

            EventSystem.current.SetSelectedGameObject(select);

            Evaluate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeLastOption()
        {
            if (loadLastOption)
                LoadLastOption();
            else
                SetLastOption(options[0]);

            ((SelectableOption)lastOption).KeepSelected(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadLastOption()
        {
            difficulty = difficultyIndex.Value;

            RewireNavigation(GetOptionFromDifficulty());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Selectable GetOptionFromDifficulty()
        {
            foreach(var option in options)
                if (options.IndexOf(option) == difficultyIndex.Value)
                    return option;

            UnityEngine.Debug.LogWarning("ALERT: Couldn't find option from Difficulty!");

            return options[0];
        }

        /// <inheritdoc/>
        public override void Evaluate()
        {
            base.Evaluate();

            if (!CheckSelectionInGroup())
                ((SelectableOption)lastOption).KeepSelected(true);
        }
    }
}