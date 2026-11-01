//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPanelManager) +
        "\n\n")]
    public class DebatePanelManager : MonoBehaviour, IPanelManager
    {
        /// <summary>
        ///
        /// </summary>
        protected DRTrialCamera trialCameraDR = null;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseManager[] phraseManagers;


        protected virtual void Awake()
        {
            var allComponents = transform.root.GetComponentsInChildren<Component>();

            phraseManagers = allComponents.OfType<IPhraseManager>().ToArray();

            trialCameraDR = FindAnyObjectByType<DRTrialCamera>();
        }


        /// <inheritdoc/>
        public virtual IEnumerator PlayPanel(Panel panel)
        {
            float panelDuration = 0;

            if (panel.Phrases.Count == 0)
                panelDuration = trialCameraDR.CameraAnimator.GetCurrentAnimatorStateInfo(0).length;
            else
                foreach (var manager in phraseManagers)
                    panelDuration += manager.PlayPhrases(panel);

            // Wait for the phrase process to end.
            yield return new WaitForSeconds(panelDuration);
        }
    }
}