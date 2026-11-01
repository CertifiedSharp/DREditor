//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues.Events;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class BlurVisionEvent : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string blurVisionEventString = "BlurVision";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string stopBlurVisionEventString = "StopBlur";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Volume blurProfile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UniversalAdditionalCameraData universalData = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float focalSubtraction = 50;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float focalEndValue = 300;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float blurAnimationDuration = 1.75f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int blurAnimationLoopAmount = -1;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LoopType blurAnimationLoopType = LoopType.Yoyo;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease blurAnimationEaseCurve = Ease.InFlash;
#endif

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float stopBlurFocalLength = 150;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float stopBlurDuration = 1;

#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected Tweener blurTween = null;
#endif
        /// <summary>
        ///
        /// </summary>
        protected VolumeProfile prof;

        /// <summary>
        ///
        /// </summary>
        protected float dofValue;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            DialogueEventSystem.StartListening(blurVisionEventString, BlurVision);

            DialogueEventSystem.StartListening(stopBlurVisionEventString, StopBlur);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            DialogueEventSystem.StopListening(blurVisionEventString, BlurVision);

            DialogueEventSystem.StopListening(stopBlurVisionEventString, StopBlur);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void BlurVision(object o = null)
        {
            prof = blurProfile.profile;

            UniversalAdditionalCameraData data = universalData;

            data.renderPostProcessing = true;

            DepthOfField dof = GetDepthOfField();

            if (dof != null)
            {
                dofValue = dof.focalLength.value;

                dof.focalLength.value -= focalSubtraction;
#if DOTWEEN
                blurTween = DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, focalEndValue, blurAnimationDuration)
                    .SetLoops(blurAnimationLoopAmount, blurAnimationLoopType)
                    .SetEase(blurAnimationEaseCurve);
#endif
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void StopBlur(object o = null)
        {
#if DOTWEEN
            if (blurTween == null)
                return;

            blurTween.Kill();

            DepthOfField dof = GetDepthOfField();

            if (dof != null)
            {
                DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, stopBlurFocalLength, stopBlurDuration);

                dof.focalLength.value = dofValue;

                UniversalAdditionalCameraData data = universalData;

                data.renderPostProcessing = false;

                blurTween = null;

                blurProfile.profile = prof;
            }
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual DepthOfField GetDepthOfField()
        {
            if (blurProfile.profile.TryGet(out DepthOfField depthOfFieldComponent))
                return depthOfFieldComponent;
            else
                return null;
        }
    }
}