//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class BehaviourButtonEditor : BehaviourAttributeEditorBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected IEnumerable<MethodInfo> methods;


        public override float Priority => 1;


        public override void Initialize(UnityEngine.Object target, UnityEngine.Object[] targets)
        {
            base.Initialize(target, targets);

            InitializeMethods();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeMethods() => 
            methods = target
            .GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(method => method.GetCustomAttribute<BehaviourButtonAttribute>() != null);


        public override void Display()
        {
            if (MethodsAreValid())
                DisplayMethodButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool MethodsAreValid() => methods != null;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayMethodButtons()
        {
            foreach (MethodInfo method in methods)
                DisplayMethodButton(method);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        protected virtual void DisplayMethodButton(MethodInfo method)
        {
            string label = CreateButtonLabel(method);

            if (HandyFields.Button(label))
                foreach (var target in targets)
                    InvokeButtonFor(target, method, label);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected virtual string CreateButtonLabel(MethodInfo method)
        {
            BehaviourButtonAttribute attribute = method.GetCustomAttribute<BehaviourButtonAttribute>();

            return string.IsNullOrEmpty(attribute.Label) ? ObjectNames.NicifyVariableName(method.Name) : attribute.Label;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="method"></param>
        /// <param name="label"></param>
        protected virtual void InvokeButtonFor(UnityEngine.Object target, MethodInfo method, string label)
        {
            Undo.RecordObject(target, "Button: " + label);

            method.Invoke(target, null);

            SetTargetDirty(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        protected virtual void SetTargetDirty(UnityEngine.Object target)
        {
            EditorUtility.SetDirty(target);

            SetTargetChildrenDirty(target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        protected virtual void SetTargetChildrenDirty(UnityEngine.Object target)
        {
            if (target is Component component)
                foreach (var child in component.gameObject.GetComponentsInChildren<Component>())
                    EditorUtility.SetDirty(child);
        }
    }
}