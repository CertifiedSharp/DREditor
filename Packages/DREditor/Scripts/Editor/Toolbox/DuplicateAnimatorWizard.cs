//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class DuplicateAnimatorWizard : DuplicateWizardBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected GameObject target = null;


        /// <summary>
        /// 
        /// </summary>
        private const string MenuPath = "GameObject/DREditor/Duplicate Insert Animation";


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MenuItem(MenuPath, true)]
        private static bool VerifyAnimator() =>
            Selection.activeGameObject != null &&
            Selection.activeGameObject.TryGetComponent(out Animator animator) &&
            animator.runtimeAnimatorController != null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem(MenuPath, false, -20)]
        private static void OpenWizard(MenuCommand menuCommand)
        {
            GameObject contextObject = menuCommand.context as GameObject;

            GameObject clickedObject = contextObject != null ? contextObject : Selection.activeGameObject;

            if (clickedObject != null)
                OpenWizard(clickedObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public static void OpenWizard(GameObject target)
        {
            DuplicateAnimatorWizard wizard = DisplayWizard<DuplicateAnimatorWizard>("Duplicate Insert Animation", "Duplicate");

            wizard.SetTarget(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public virtual void SetTarget(GameObject target) => this.target = target;


        protected override bool OnEnableWizardValid() => VerifyAnimator() || ImportValid();

        protected override bool ImportValid() => target != null && target.GetComponent<Animator>() != null;

        protected override void Import()
        {
            if (target.TryGetComponent(out Animator animator))
                DuplicateInsertAnimator(animator);
            else
                Debug.LogWarning("ALERT: No Animator on selected object.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        protected virtual void DuplicateInsertAnimator(Animator animator)
        {
            animator.runtimeAnimatorController = DuplicateControllerFrom(GetControllerFrom(animator));

            EditorUtility.SetDirty(animator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <returns></returns>
        protected virtual AnimatorController GetControllerFrom(Animator animator)
        {
            string controllerPath = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);

            return AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        protected virtual AnimatorController DuplicateControllerFrom(AnimatorController controller)
        {
            if (TryDuplicateAsset(controller, out AnimatorController duplicateController))
                MergeAnimators(controller, duplicateController);
            else
                Debug.LogWarning("ALERT: Loaded Controller Null Or Duplicated Asset Failure.");

            return duplicateController;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="duplicateController"></param>
        protected virtual void MergeAnimators(AnimatorController controller, AnimatorController duplicateController)
        {
            List<string> cache = new();

            foreach (AnimationClip clip in controller.animationClips)
                PopulateDuplicatedAnimatorClips(duplicateController, clip, cache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duplicateController"></param>
        /// <param name="clip"></param>
        /// <param name="cache"></param>
        protected virtual void PopulateDuplicatedAnimatorClips(AnimatorController duplicateController, AnimationClip clip, List<string> cache)
        {
            foreach (AnimatorControllerLayer layer in duplicateController.layers)
                foreach (ChildAnimatorState childState in layer.stateMachine.states)
                    if (StateValid(childState) && TryDuplicatingValidClip(childState, clip, cache))
                        Apply(childState, clip, cache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childState"></param>
        /// <param name="clip"></param>
        /// <param name="cache"></param>
        protected virtual void Apply(ChildAnimatorState childState, AnimationClip clip, List<string> cache)
        {
            string clipPath = BuildDuplicatedPath(clip.name, AssetDatabase.GetAssetPath(clip));

            var dupedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

            if (!cache.Contains(clip.name))
                cache.Add(clip.name);

            childState.state.motion = dupedClip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childState"></param>
        /// <returns></returns>
        protected virtual bool StateValid(ChildAnimatorState childState) =>
            childState.state != null && childState.state.motion != null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childState"></param>
        /// <param name="clip"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        protected virtual bool TryDuplicatingValidClip(ChildAnimatorState childState, AnimationClip clip, List<string> cache) =>
            clip != null && clip.name == childState.state.motion.name && (cache.Contains(clip.name) || TryDuplicateAsset(clip, out _));
    }
}