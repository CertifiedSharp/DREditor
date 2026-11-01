//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI.ReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ReportCardControllableComponent) +
        "\n\n")]
    public class CharacterReportCard : ReportCardControllableComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleLeft = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleRight = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string talentPrefix = "Ultimate ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string shuffleLeftActionName = "ShuffleLeft ";

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI headerName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI headerTalent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textDob;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textHeight;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textLikes;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textDislikes;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textAge;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textWeight;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected Image imgMugshot;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image imgSignature;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<ReportCardComponent> components = null;


        /// <summary>
        ///
        /// </summary>
        protected List<ReportCardControllableComponent> controllableComponents = new();


        protected virtual void Start()
        {
            foreach (var component in components)
                if (component is ReportCardControllableComponent controllable)
                    controllableComponents.Add(controllable);
        }


        /// <inheritdoc/>
        public override void ActivateControls()
        {
            shuffleLeft.action.performed += ChangeCard;

            shuffleRight.action.performed += ChangeCard;

            foreach (var component in controllableComponents)
                if (component != null)
                {
                    component.SuccessfulUINavigation += OnUINavigated;

                    component.ActivateControls();
                }
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            shuffleLeft.action.performed -= ChangeCard;

            shuffleRight.action.performed -= ChangeCard;

            foreach (var component in controllableComponents)
                if (component != null)
                {
                    component.SuccessfulUINavigation -= OnUINavigated;

                    component.DeactivateControls();
                }
        }

        /// <inheritdoc/>
        public override void ResetComponent() => ChangeCard(0);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ChangeCard(CallbackContext context)
        {
            if (!context.performed)
                return;

            int dir; // direction for the pages to move. -1 = left, 1 = right.

            if (context.action.name == shuffleLeftActionName)
                dir = -1;
            else
                dir = 1;

            if (cardIndex + dir >= 0 && cardIndex + dir <= characterDatabase.GetFTECharacterCount())
            {
                ChangeCard(cardIndex + dir);

                OnUINavigated();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        protected virtual void ChangeCard(int index)
        {
            try
            {
                SetCardIndex(index);

                UpdateReportCardUI();
            }
            catch (IndexOutOfRangeException)
            {
                UnityEngine.Debug.LogError("ALERT: ERROR: When running ReportCardMenu.ChangeCard(" + index.ToString()
                    + "). Index out of bounds. Resetting to default selection...");

                ResetComponent();
            }

            foreach (var component in controllableComponents)
                if (component != null)
                    component.ResetComponent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateReportCardUI()
        {
            Student student = (Student)character;

            if (headerName != null)
                headerName.text = (student.FirstName + " " + student.LastName).ToUpper();

            if (headerTalent != null)
                headerTalent.text = talentPrefix + student.StudentCard.Talent;

            if (textDob != null)
                textDob.text = student.StudentCard.DateOfBirth;

            if (textHeight != null)
                textHeight.text = student.StudentCard.Height;

            if (textLikes != null)
                textLikes.text = student.StudentCard.Likes;

            if (textDislikes != null)
                textDislikes.text = student.StudentCard.Dislikes;

            if (textAge != null)
                textAge.text = student.StudentCard.Age;

            if (textWeight != null)
                textWeight.text = student.StudentCard.Weight;

            if (imgMugshot != null && student.StudentCard.Picture != null)
                imgMugshot.sprite = CreateSprite(student.StudentCard.Picture);

            if (imgSignature != null && student.StudentCard.Signature != null)
                imgSignature.sprite = CreateSprite(student.StudentCard.Signature);
        }

        /// <summary>
        ///
        /// </summary>
        private Sprite CreateSprite(Texture2D texture)
            => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        


        /// <inheritdoc/>
        public override void SetCardIndex(int cardIndex)
        {
            base.SetCardIndex(cardIndex);

            foreach (var component in components)
                component.SetCardIndex(cardIndex);
        }

        /// <inheritdoc/>
        public override void SetCharacterNames(List<string> names)
        {
            base.SetCharacterNames(names);

            foreach (var component in components)
                component.SetCharacterNames(names);
        }
    }
}