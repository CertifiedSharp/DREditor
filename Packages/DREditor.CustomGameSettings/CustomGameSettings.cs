//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
#if NEWTONSOFT_JSON
using Newtonsoft.Json;
#endif
using UnityEngine;

namespace DREditor.CustomSettings
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CustomGameSettings
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string ID = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [TextArea]
        public string data = "";

        public CustomGameSettings(string ID, string data)
        {
            this.ID = ID;

            this.data = data;
        }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JObjectSerialize<T>(T data) where T : ScriptableObject
        {
            string result;
#if NEWTONSOFT_JSON
            result = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Auto
                });
#else
            result = JsonUtility.ToJson(data);
#endif
            result = result.Replace("\r\n", "");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JObjectDeserialize<T>(string json) where T : ScriptableObject
        {
#if NEWTONSOFT_JSON
            var instance = ScriptableObject.CreateInstance<T>();

            JsonConvert.PopulateObject(json, instance);

            return instance;
#else
            return JsonUtility.FromJson<T>(json);
#endif
        }
    }
}