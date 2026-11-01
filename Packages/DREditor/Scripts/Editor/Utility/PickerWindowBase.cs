//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PickerWindowBase<T> : EditorWindow
    {
        /// <summary>
        ///
        /// </summary>
        protected SerializedObject targetObject;

        /// <summary>
        ///
        /// </summary>
        protected string propertyPath;

        /// <summary>
        ///
        /// </summary>
        protected T current;

        /// <summary>
        ///
        /// </summary>
        protected readonly Color selectedBackgroundColor = new(0.24f, 0.48f, 0.90f, 0.35f);

        /// <summary>
        ///
        /// </summary>
        protected readonly Color hoverBackgroundColor = new(0.5f, 0.5f, 0.5f, 0.12f);

        /// <summary>
        ///
        /// </summary>
        protected readonly Vector2 minimumWindowSize = new(340, 420);


        /// <summary>
        ///
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="propertyPath"></param>
        /// <param name="currentValue"></param>
        public virtual void Show(SerializedObject targetObject, string propertyPath, T currentValue)
        {
            minSize = minimumWindowSize;

            this.targetObject = targetObject;

            this.propertyPath = propertyPath;

            current = currentValue;

            Initialize();

            ShowUtility();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void Initialize();
    }
}
#endif