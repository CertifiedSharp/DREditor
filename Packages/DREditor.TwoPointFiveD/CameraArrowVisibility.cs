//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class CameraArrowVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent allArrowVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<CameraArrow> arrows = new();


        /// <summary>
        ///
        /// </summary>
        protected bool canChange = false;


        /// <summary>
        ///
        /// </summary>
        protected override void ShowUI() => StartCoroutine(Appear());

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual IEnumerator Appear()
        {
            if (isLoadingFile.Value)
                yield break;

            if (inLeaveProcess.Value)
            {
                UpdateArrowStates();

                yield break;
            }

            canChange = true;

            allArrowVisibility.Show();

            yield return allArrowVisibility.WaitForChangeToComplete();

            UpdateArrowStates();
        }


        /// <summary>
        ///
        /// </summary>
        protected override void HideUI() => StartCoroutine(Disappear());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator Disappear()
        {
            StopCoroutine(Appear());

            canChange = false;

            allArrowVisibility.Hide();

            yield return allArrowVisibility.WaitForChangeToComplete();

            UpdateArrowStates();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateArrowStates()
        {
            foreach (CameraArrow arrow in arrows)
                arrow.SetCanChange(canChange);
        }


        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            yield return allArrowVisibility.WaitForChangeToComplete();
        }
    }
}