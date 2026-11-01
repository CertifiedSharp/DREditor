//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MapUITab) + ", " + nameof(IPlaceOptionHolder) +
        "\n\n")]
    public class PlaceMapUITab : MapUITab, IPlaceOptionHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MapAreaManager currentAreaManager = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameObject placeTabHighlight = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CanvasGroup placeGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<TextMeshProUGUI> placeOptions = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int startingOptionIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int endingOptionDifference = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string fastTravelUIPrefix = "[JMP] ";


        /// <summary>
        ///
        /// </summary>
        protected List<Selectable> selectOptions = new();


        protected virtual void Start() => InitializeSelectables();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSelectables()
        {
            foreach (TextMeshProUGUI op in placeOptions)
                selectOptions.Add(op.GetComponent<Selectable>());
        }


        /// <inheritdoc/>
        public override void InitializeTab()
            => EventSystem.current.SetSelectedGameObject(placeOptions[startingOptionIndex].gameObject);

        /// <inheritdoc/>
        public override void HideTab()
        {
            placeGroup.alpha = 0;

            placeTabHighlight.SetActive(false);
        }

        /// <inheritdoc/>
        public override void ShowTab()
        {
            placeGroup.alpha = 1;

            placeTabHighlight.SetActive(true);
        }


        /// <inheritdoc/>
        public virtual void PopulatePlaceUIText(int shiftNum, List<string> placeNames)
        {
            ClearPlaceOptionText();

            if (currentAreaManager == null)
                currentAreaManager = transform.root.GetComponentInChildren<MapAreaManager>();

            for (int i = 0; i < placeOptions.Count; i++)
                placeOptions[i].text = GetDisplayText(i, shiftNum, placeNames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="shiftNum"></param>
        /// <param name="placeNames"></param>
        /// <returns></returns>
        protected virtual string GetDisplayText(int i, int shiftNum, List<string> placeNames)
        {
            int idx = i + shiftNum - startingOptionIndex;

            if ((i < startingOptionIndex && shiftNum == 0) || (idx > placeNames.Count - 1))
                return "";

            if (currentAreaManager != null &&
                currentAreaManager.GetPlaceFromName(placeNames[idx]) is PlaceManager m &&
                m.Data.fastTravelable)
                return fastTravelUIPrefix + placeNames[idx];

            return placeNames[idx];
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearPlaceOptionText()
        {
            for (int i = 0; i < placeOptions.Count; i++)
                placeOptions[i].text = "";
        }

        /// <inheritdoc/>
        public virtual PlaceManager GetCurrentlyHoveredPlace()
        {
            if (currentAreaManager == null)
                currentAreaManager = ServiceLocator.GetService<MapAreaManager>();

            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            TextMeshProUGUI currentText = selectedObject != null ? selectedObject.GetComponent<TextMeshProUGUI>() : null;

            if (currentText == null && placeOptions.Count > startingOptionIndex)
                currentText = placeOptions[startingOptionIndex];

            if (currentText == null || currentAreaManager == null || !placeOptions.Contains(currentText))
            {
                UnityEngine.Debug.LogWarning("WARNING!: Either the hovered place on the UI is not a valid place in the games data," +
                " or the specified place couldn't be found, or the current area manager was null.");

                return null;
            }

            string placeName = currentText.text.Replace(fastTravelUIPrefix, "");

            return currentAreaManager.GetPlaceFromName(placeName);
        }

        /// <inheritdoc/>
        public virtual int GetMaxShiftIndex()
            => currentAreaManager.Data.places.Count - placeOptions.Count + startingOptionIndex + endingOptionDifference;

        /// <inheritdoc/>
        public virtual GameObject GetTopOption() => placeOptions[startingOptionIndex].gameObject;

        /// <inheritdoc/>
        public virtual GameObject GetBottomOption() => placeOptions[^(1 + endingOptionDifference)].gameObject;

        /// <inheritdoc/>
        public virtual void EnableSelectablesOnValidText()
        {
            DisableSelectableOptions();

            for (int i = startingOptionIndex; i < placeOptions.Count - endingOptionDifference; i++)
                placeOptions[i].GetComponent<Selectable>().enabled = placeOptions[i].text != "";
        }

        /// <inheritdoc/>
        public virtual void DisableSelectableOptions()
        {
            for (int i = 0; i < selectOptions.Count; i++)
                selectOptions[i].enabled = false;
        }
    }
}