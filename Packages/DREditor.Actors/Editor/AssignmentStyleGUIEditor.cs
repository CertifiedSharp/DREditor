//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class AssignmentStyleGUIEditor<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<T> objects = new();

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int[] assignments;


        /// <summary>
        /// 
        /// </summary>
        public List<T> Objects { get => objects; }

        /// <summary>
        /// 
        /// </summary>
        public int[] Assignments { get => assignments; }


        /// <summary>
        /// 
        /// </summary>
        protected readonly int objectCount;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Func<T, string> GetObjectName;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string NoneText = "(None)";


        public AssignmentStyleGUIEditor(List<T> objects, Func<T, string> GetObjectName, int objectCount, int[] assignments = null)
        {
            this.objects = objects;

            this.GetObjectName = GetObjectName;

            this.objectCount = objectCount;

            if (assignments != null)
            {
                this.assignments = new int[objectCount];

                for (int i = 0; i < this.assignments.Length; i++)
                    this.assignments[i] = i < assignments.Length ? assignments[i] : i;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void ValidateAssignments()
        {
            CheckNullAssignments();

            ValidateObjectAmounts();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CheckNullAssignments()
        {
            if (assignments != null)
                return;

            assignments = new int[objectCount];

            for (int i = 0; i < assignments.Length; i++)
                assignments[i] = i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateObjectAmounts()
        {
            bool result = ObjectAmountsValid();

            if (!result)
                OnObjectAmountsInvalid();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ObjectAmountsValid() => objectCount > 0 && objects.Count > 0;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnObjectAmountsInvalid() =>
            EditorGUILayout.HelpBox($"ALERT: You must have {typeof(T).Name}s to use this editor!", MessageType.Warning);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetAssignedNames()
        {
            string[] names = new string[objectCount];

            for (int i = 0; i < objectCount && i < objects.Count; i++)
                names[i] = GetName(objects[i]);

            return names;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual string GetName(T obj) => string.IsNullOrEmpty(GetObjectName(obj)) ? NoneText : GetObjectName(obj);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="newIndex"></param>
        public virtual void SwapAssignment(int slot, int newIndex)
        {
            int oldIndex = assignments[slot];

            if (oldIndex == newIndex)
                return;

            for (int other = 0; other < assignments.Length; other++)
            {
                if (other != slot && assignments[other] == newIndex)
                {
                    assignments[other] = oldIndex;

                    break;
                }
            }

            assignments[slot] = newIndex;
        }
    }
}