//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ActorSpriteFader) + ", " + nameof(ITweenWaiter) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ActorSpriteFaderRenderQ : ActorSpriteFader
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected StringWithEvent renderQCamCode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string renderQCameraCode = "Fading";


        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public override IEnumerator Fading(Actor actor, float to)
        {
            renderQCamCode.SetValue(renderQCameraCode);

            yield return base.Fading(actor, to);

            if (renderQCamCode.Value == renderQCameraCode)
                renderQCamCode.Invoke();
            else
                renderQCamCode.SetValue(renderQCameraCode);
        }
    }
}