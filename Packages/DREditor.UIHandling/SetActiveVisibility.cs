//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class SetActiveVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<GameObject> objects = new();


        /// <inheritdoc/>
        protected override void ShowUI()
        {
            foreach (var obj in objects)
                obj.SetActive(true);
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
            foreach (var obj in objects)
                obj.SetActive(false);
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            yield break;
        }
    }
}