//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlsUITab) +
        "\n\n")]
    public class KeysUITab : ControlsUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform rowParent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject rowPrefab;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<GameObject> controlRows;


        /// <inheritdoc/>
        protected override bool FailedVerification()
        {
            bool result = rowPrefab == null && (controlRows.Count == 0 || controlRows[0] == null);

            if (result)
                UnityEngine.Debug.LogWarning($"ALERT: Could not display: {nameof(KeysUITab)} because it's control rows " +
                    $"were either empty or the filled out rows are null.");

            return result;
        }

        /// <inheritdoc/>
        protected override void DisplayActive() => DisplayRows();

        /// <inheritdoc/>
        protected override void DisplayInactive() { }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRows()
        {
            foreach(GameObject row in controlRows)
                if (row != null)
                    Destroy(row);

            controlRows.Clear();

            for (int i = 0; i < activePanel.Rows.Length; i++)
                controlRows.Add(Instantiate(rowPrefab, rowParent));

            for (int i = 0; i < activePanel.Rows.Length; i++)
                if (i >= activePanel.Rows.Length)
                    controlRows[i].SetActive(false);
                else
                    DisplayRow(controlRows[i].transform, i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rowTransform"></param>
        /// <param name="i"></param>
        protected virtual void DisplayRow(Transform rowTransform, int i)
        {
            rowTransform.gameObject.SetActive(true);

            if (rowTransform.TryGetComponent(out IControlsUIRow row))
                row.DisplayRow(activePanel.Rows[i]);
        }
    }
}