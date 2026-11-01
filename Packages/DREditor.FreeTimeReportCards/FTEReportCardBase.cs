//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FreeTime;
using DREditor.UI.ReportCards;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ReportCardControllableComponent) +
        "\n\n")]
    public abstract class FTEReportCardBase : ReportCardControllableComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference navigate = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI textFTE;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useHorizontalInput = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int maxEventsPerCharacter = 5;


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent<int> OnFTETabsUpdated = null;


        [Header("")]
        [SerializeField] protected UnityEvent<int> OnFTEIndexUpdated = null;


        /// <summary>
        ///
        /// </summary>
        protected IFTEDataHandler fteData = null;

        /// <summary>
        ///
        /// </summary>
        protected int fteIndex = 0;

        


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            fteData = FindObjectsByType<MonoBehaviour>().OfType<IFTEDataHandler>().FirstOrDefault();

            if (fteData == null)
                UnityEngine.Debug.LogWarning(nameof(FTEReportCardBase) + ": " +
                    "ALERT: The FTE data handler must be available for this script to operate." +
                    "\nPROPOSAL: Ensure that an object that inherits the type: " + nameof(IFTEDataHandler) + " is in the scene.");
        }


        /// <inheritdoc/>
        public override void SetCardIndex(int cardIndex)
        {
            base.SetCardIndex(cardIndex);

            UpdateUnlockedFTETabs();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateUnlockedFTETabs() => OnFTETabsUpdated?.Invoke(GetCharacterAffinity());


        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ChangeFTE(CallbackContext context)
        {
            int inputAxis = useHorizontalInput ? (int)context.ReadValue<Vector2>().x : (int)context.ReadValue<Vector2>().y;

            if (!context.performed || inputAxis == 0)
                return;

            // direction for the fte to move. -1 = up, 1 = down.
            int dir = inputAxis * -1; 

            if (fteIndex + dir >= 0 &&
                fteIndex + dir < maxEventsPerCharacter + 1 &&
                ChangeFTE(fteIndex + dir))
                OnUINavigated();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual bool ChangeFTE(int index)
        {
            bool affinityValid = index >= 0 && index <= GetCharacterAffinity();

            bool result = !character.NotFTECharacter && affinityValid;

            if (result)
            {
                fteIndex = index;

                DisplayFTETabData();
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFTETabData()
        {
            OnFTEIndexUpdated?.Invoke(fteIndex);

            if (fteIndex > 0 && character.Has(out SimpleFTEEntryCollection collection))
                textFTE.text = collection[fteIndex - 1];
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCharacterAffinity()
            => fteData == null || character.NotFTECharacter ? 0 : fteData.GetAffinity(character.FirstName);


        /// <inheritdoc/>
        public override void ActivateControls() => navigate.action.performed += ChangeFTE;

        /// <inheritdoc/>
        public override void DeactivateControls() => navigate.action.performed -= ChangeFTE;

        /// <inheritdoc/>
        public override void ResetComponent() => ChangeFTE(0);
    }
}