//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    public static class DRESaveDictionaryExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this Dictionary<string, object> data, string key, T defaultValue = default)
        {
            try
            {
                if (data.TryGetValue(key, out object value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException invalidCast)
            {
                UnityEngine.Debug.Log($"ANALYSIS: Save key: {key} was not of type {typeof(T).Name}" +
                    $"\nHYPOTHESIS: The system may be attempting to check for an older save data type first " +
                    $"before converting the proper type. In this event, this log can be ignored." +
                    $"\n{invalidCast}");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"ANALYSIS: Something's wrong with converting data for key: {key}");

                UnityEngine.Debug.LogWarning(e.ToString());
            }

            return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object GetGeneric(this Dictionary<string, object> data, string key, Type type, object defaultValue = null)
        {
            try
            {
                if (data.TryGetValue(key, out object value))
                    return Convert.ChangeType(value, type);
            }
            catch (InvalidCastException invalidCast)
            {
                UnityEngine.Debug.Log($"ANALYSIS: Save key: {key} was not of type {type.Name}" +
                    $"\nHYPOTHESIS: The system may be attempting to check for an older save data type first " +
                    $"before converting the proper type. In this event, this log can be ignored." +
                    $"\n{invalidCast}");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"ANALYSIS: Something's wrong with converting data for key: {key}");

                UnityEngine.Debug.LogWarning(e.ToString());
            }

            return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this Dictionary<string, object> data, string key, T value)
        {
            if (data.ContainsKey(key))
                UnityEngine.Debug.LogWarning($"DATA KEY: {key} is being REPLACED!");

            data[key] = value;
        }
    }
}