//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<string> GetFieldNames<T>(this T obj)
        {
            List<string> fieldNames = new ();

            CollectFieldNames(obj, fieldNames);

            return fieldNames;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldNames"></param>
        private static void CollectFieldNames(object obj, List<string> fieldNames)
        {
            if (obj == null) 
                return;

            Type type = obj.GetType();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                fieldNames.Add(field.Name);

                object value = field.GetValue(obj);

                if (value != null && !IsPrimitiveOrString(value.GetType()))
                    CollectFieldNames(value, fieldNames);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<object> GetFieldValues<T>(this T obj)
        {
            List<object> fieldValues = new();

            CollectFieldValues(obj, fieldValues);

            return fieldValues;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldValues"></param>
        private static void CollectFieldValues(object obj, List<object> fieldValues)
        {
            if (obj == null) 
                return;

            Type type = obj.GetType();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                object value = field.GetValue(obj);

                fieldValues.Add(value);

                if (value != null && !IsPrimitiveOrString(value.GetType()))
                    CollectFieldValues(value, fieldValues);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T CreateInstanceFromDictionary<T>(Dictionary<string, object> data) where T : new()
            => (T)CreateInstanceFromDictionary(typeof(T), data);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object CreateInstanceFromDictionary(Type type, Dictionary<string, object> data)
        {
            object instance = Activator.CreateInstance(type);

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
                if (data.TryGetValue(field.Name, out var value))
                    if (value == null || IsPrimitiveOrString(field.FieldType))
                        field.SetValue(instance, value);
                    else
                        field.SetValue(instance, CreateInstanceFromDictionary(field.FieldType, data));

            return instance;
        }
    }
}