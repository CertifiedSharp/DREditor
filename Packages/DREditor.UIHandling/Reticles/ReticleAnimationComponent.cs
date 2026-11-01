//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ReticleAnimationComponent : ReticleComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string hoverBoolName = "IsSelected";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string confirmSelectBoolName = "ConfirmSelect";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string appearBoolName = "Appear";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<ReticleIconLayer> iconBools = new() 
        { new ReticleIconLayer("SpeakIcon"), new ReticleIconLayer("ItemIcon") };

        
        /// <summary>
        ///
        /// </summary>
        protected Animator animator;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => InitializeAnimator();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeAnimator()
        {
            if (!animator)
                animator = GetComponent<Animator>();

            if (!animator)
                animator = GetComponentInParent<Animator>();
        }

        /// <inheritdoc/>
        public override void ReticleVisibilityChanged(bool to)
        {
            if (!animator)
                InitializeAnimator();

            animator.SetBool(appearBoolName, to);
        }

        /// <inheritdoc/>
        public override void OnHoveringObject(Transform hit)
        {
            HideAllIcons();

            HoverVisual(GetIconName(hit.gameObject));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual string GetIconName(GameObject obj)
        {
            foreach (var icon in iconBools)
                if (IsInLayerMask(obj, icon.mask))
                    return icon.iconBool;

            return "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        protected virtual bool IsInLayerMask(GameObject go, LayerMask mask) => (mask.value & (1 << go.layer)) != 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="icon"></param>
        protected virtual void HoverVisual(string icon = "")
        {
            animator.SetBool(hoverBoolName, true);

            animator.SetBool(appearBoolName, true);

            if (icon != "")
                animator.SetBool(icon, true);
        }

        /// <inheritdoc/>
        public override void OnSelected(Transform hit) => animator.SetBool(confirmSelectBoolName, true);

        /// <inheritdoc/>
        public override void OnStoppedHoveringObject(Transform hit)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
                if (parameter.name == appearBoolName)
                    continue;
                else
                    animator.SetBool(parameter.name, false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideAllIcons()
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
                foreach(var bools in iconBools)
                    if (bools.iconBool == parameter.name)
                        animator.SetBool(parameter.name, false);
        }
    }
}