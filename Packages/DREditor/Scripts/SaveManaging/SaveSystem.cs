//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
#if NEWTONSOFT_JSON
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
#endif
using UnityEngine;
using DREditor.Saving;

namespace DREditor.SaveManaging
{
    /// <summary>
    ///
    /// </summary>
    public static class SaveSystem
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toSave"></param>
        /// <param name="filename"></param>
        public static void SaveToJSON<T>(List<T> toSave, string filename)
        {
            UnityEngine.Debug.Log(GetFilePath(filename));

            string content = JsonHelper.ToJson(toSave.ToArray());

            WriteFile(GetFilePath(filename), content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toSave"></param>
        /// <param name="filename"></param>
        public static void SaveToJSON<T>(T toSave, string filename)
        {
            string content = JsonUtility.ToJson(toSave, true);

            WriteFile(GetFilePath(filename), content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<T> ReadListFromJSON<T>(string filename)
        {
            string content = ReadFile(GetFilePath(filename));

            if (string.IsNullOrEmpty(content) || content == "{}")
                return new List<T>();

            List<T> res = JsonHelper.FromJson<T>(content).ToList();

            return res;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T ReadFromJSON<T>(string filename)
        {
            string content = ReadFile(GetFilePath(filename));
            
            if (string.IsNullOrEmpty(content) || content == "{}")
                return default;

            content = content.Replace("\"Info\":", "\"Infos\":");

            content = content.Replace("\"DataList\":", "\"dataSets\":");

            content = content.Replace("\"iData\":", "\"itemDialogues\":");

            content = content.Replace("\"aData\":", "\"actorDialogues\":");

            content = content.Replace("\"actorList\":", "\"actorDatas\":");

            content = content.Replace("\"itemList\":", "\"itemDatas\":");

            content = content.Replace("\"spawnList\":", "\"spawnDatas\":");

            content = content.Replace("DREditor.Progression", "DREditor.Progressing");

            T res;

            if (typeof(T) == typeof(SerializableDictionary))
            {
                res = JsonUtility.FromJson<T>(content);

                if (res == null)
                    UnityEngine.Debug.LogWarning("Issue with ReadingFromJSON on file: " + filename);
            }
            else
            {
#if NEWTONSOFT_JSON
                // We needed to use the newtonsoft json because when Converting old save file data to the new one,
                // that way, the system can attempt to salvage corrupted or outdated save data.

                // Load as JObject
                JObject jObject = JObject.Parse(content);

                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new IgnoreTypesResolver(),
                };

                // Now deserialize
                res = jObject.ToObject<T>(JsonSerializer.Create(settings));
#else
                UnityEngine.Debug.LogError("ALERT: Failed to Load Save file.");

                UnityEngine.Debug.LogWarning("HYPOTHESIS: Since the user does not have the NEWTONSOFT dependency. " +
                    "It is likely that the data attempting to be loaded is paradoxically outdated. " +
                    "Which was originally the purpose for the dependency.");

                UnityEngine.Debug.LogWarning("PROPOSAL: Download the NEWTONSOFT dependency.");

                return default;
#endif
            }

            return res;
        }
#if NEWTONSOFT_JSON
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        sealed class IgnoreTypesResolver : DefaultContractResolver
        {
            private static readonly HashSet<Type> IgnoredTypes = new()
            {
                typeof(Material),
                typeof(Texture),
                typeof(GameObject),
                typeof(AudioClip),
                typeof(Transform),
                typeof(ScriptableObject),
                typeof(Dialogue),
                typeof(Character)
            };

            /// <summary>
            ///
            /// </summary>
            /// <param name="member"></param>
            /// <param name="serialization"></param>
            /// <returns></returns>
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization serialization)
            {
                var prop = base.CreateProperty(member, serialization);

                if (IgnoredTypes.Contains(prop.PropertyType))
                    prop.Ignored = true;

                return prop;
            }
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool SaveFileExists(string filename)
        {
            string path = GetFilePath(filename);

            if (File.Exists(path))
                return IsFileContentValid(path);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsFileContentValid(string path)
        {
            string content = ReadFile(path);

            bool contentValid = !(string.IsNullOrEmpty(content) || content == "{}");

            return contentValid;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string GetFilePath(string filename) => Application.persistentDataPath + "/" + filename;

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        private static void WriteFile(string path, string content)
        {
            FileStream fileStream = new(path, FileMode.Create);

            using StreamWriter writer = new(fileStream);

            writer.Write(content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string ReadFile(string path)
        {
            if (File.Exists(path))
            {
                using StreamReader reader = new(path);

                string content = reader.ReadToEnd();

                return content;
            }

            return "";
        }
    }
}