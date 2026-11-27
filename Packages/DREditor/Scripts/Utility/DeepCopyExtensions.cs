//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Reflection;
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Class for extensions on objects containing different ways to make deep copies of objects; <br/>
    /// Since different classes may require different processes in order to properly make them.
    /// </summary>
    public static class DeepCopyExtensions
    {
        /// <summary>
        /// Creates a deep copy of <typeparamref name="T"/> based on whether it's a data/value type, array, or custom object.
        /// </summary>
        /// <typeparam name="T">The type of object to be deep copied.</typeparam>
        /// <param name="obj">The object to be deep copied.</param>
        /// <returns>An untethered copy of <typeparamref name="T"/>.</returns>
        public static T DeepCopy<T>(this T obj)
        {
            if (obj == null)
                return default;

            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
                return obj;
            else if (type.IsArray)
                return DeepCopyArray(obj);
            else
                return DeepCopyObject(obj);
        }

        /// <summary>
        /// Manually creates a deep copy of the <typeparamref name="T"/> array.
        /// </summary>
        /// <typeparam name="T">The type of object to be deep copied.</typeparam>
        /// <param name="obj">The object to be deep copied.</param>
        /// <returns>An untethered copy of the <typeparamref name="T"/> array.</returns>
        private static T DeepCopyArray<T>(T obj)
        {
            Type type = obj.GetType();

            Type elementType = type.GetElementType();

            var array = obj as Array;

            Array copied = Array.CreateInstance(elementType, array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                var copiedField = array.GetValue(i) is ScriptableObject ? array.GetValue(i) : DeepCopy(array.GetValue(i));

                copied.SetValue(copiedField, i);
            }

            return (T)(object)copied;
        }

        /// <summary>
        /// Creates a deep copy of <typeparamref name="T"/> using field info to manually copy over the fields.
        /// </summary>
        /// <typeparam name="T">The type of object to be deep copied.</typeparam>
        /// <param name="obj">The object to be deep copied.</param>
        /// <returns>A new untethered copy of <paramref name="obj"/>.</returns>
        /// <remarks>ALERT: 
        /// <see cref="Texture2D"/>'s are not allowed to be copied and will report an error while simply returning the sent object.</remarks>
        private static T DeepCopyObject<T>(T obj)
        {
            Type type = obj.GetType();

            if (obj is Texture2D texture)
            {
                UnityEngine.Debug.LogError($"ALERT: CAUGHT A TEXTURE2D Instance: " + texture.name);

                return obj;
            }

            object copy = Activator.CreateInstance(type);

            FieldInfo[] fields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(obj);

                if (fieldValue != null)
                {
                    var copiedField = fieldValue is ScriptableObject ? fieldValue : DeepCopy(fieldValue);

                    field.SetValue(copy, copiedField);
                }
            }

            return (T)copy;
        }


        /// <summary>
        /// Creates a deep copy instance of an Scriptable Object using JSON.
        /// </summary>
        /// <typeparam name="T">The type of scriptable object to make a copy of.</typeparam>
        /// <param name="source">The Scriptable Object to make a copy of.</param>
        /// <returns>A new untethered copy of <paramref name="source"/>.</returns>
        /// <remarks>
        /// ANALYSIS: It is assumed this only works if <typeparamref name="T"/> does not contain any Scriptable Object fields.
        /// </remarks>
        public static T DeepJsonCopy<T>(this T source) where T : ScriptableObject
        {
            if (source == null) 
                return null;

            string json = JsonUtility.ToJson(source);

            T clone = ScriptableObject.CreateInstance<T>();

            JsonUtility.FromJsonOverwrite(json, clone);

            return clone;
        }
    }
}