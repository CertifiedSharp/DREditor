//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BehaviourAttributeEditorBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected UnityEngine.Object target;

        /// <summary>
        /// 
        /// </summary>
        protected UnityEngine.Object[] targets;


        /// <summary>
        /// 
        /// </summary>
        public abstract float Priority { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targets"></param>
        public virtual void Initialize(UnityEngine.Object target, UnityEngine.Object[] targets)
        {
            this.target = target;

            this.targets = targets;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Display();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="display"></param>
        protected virtual void Display<T>(Action<T> display)
        {
            var attributes = target.GetType().GetCustomAttributes(typeof(T), true);

            foreach (var attribute in attributes)
                if (attribute is T attributeType)
                    display(attributeType);
        }
    }
}