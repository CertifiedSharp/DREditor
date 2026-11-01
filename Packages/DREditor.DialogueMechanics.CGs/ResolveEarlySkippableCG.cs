//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ResolveEarlySkippableCG : ResolveEarlyCG
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent toSkipHideCG = null;


        /// <inheritdoc/>
        public override IEnumerator ResolveEarly()
        {
            if (!toSkipHideCG.Value)
                yield return base.ResolveEarly();
            else
                toSkipHideCG.SetValue(false);

            yield break;
        }


        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("HideCG");
    }
}