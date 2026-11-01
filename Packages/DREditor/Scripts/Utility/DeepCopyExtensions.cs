//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Reflection;
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class DeepCopyExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static T DeepCopyArray<T>(T obj)
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
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static T DeepCopyObject<T>(T obj)
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
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
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