//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if NEWTONSOFT_JSON
using Newtonsoft.Json;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SerializableDictionaryEntry
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string key;

        /// <summary>
        ///
        /// </summary>
        [TextArea]
        [Tooltip("")]
        public string value;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string type;


        /// <summary>
        ///
        /// </summary>
        const string GenericListPrefix = "System.Collections.Generic.List`1[[";


        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        private static readonly List<string> IgnoredKeys = new()
        {
            "ProgressionGate", "currentDynamicRoom", "_syncRoot", "_buckets", "_entries", "_comparer", "_keys", "_values"
        };

        /// <summary>
        ///
        /// </summary>
        private static readonly List<string> SearchableAssemblies = new() { "DREditor" };

        /// <summary>
        ///
        /// </summary>
        private static readonly Dictionary<string, Type> TypeNames = new();

        /// <summary>
        ///
        /// </summary>
        private static readonly Dictionary<string, Type> MovedTypes = new();

        /// <summary>
        ///
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <example>
        /// <code>
        /// using DREditor.Saving;
        /// using UnityEngine;
        /// 
        /// public static class SaveSystemInitializer
        /// {
        ///     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        ///     static void RegisterExtraAssemblies()
        ///     {
        ///         // Add your new assembly name here
        ///         SerializableDictionaryEntry.RegisterAssembly("MyProjectPlugin");
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void RegisterAssembly(string assemblyName)
        {
            if (!SearchableAssemblies.Contains(assemblyName))
                SearchableAssemblies.Add(assemblyName);
        }

        /// <summary>
        ///
        /// </summary>
        public static void CacheTypesFromRegisteredAsseblies()
        {
            TypeNames.Clear();

            BuildTypeNameCache();

            MovedTypes.Clear();

            BuildMovedTypeCache();
        }


        public SerializableDictionaryEntry(string key, object value)
        {
            this.key = key;

            if (value == null)
            {
                this.value = null;

                if (!IgnoredKeys.Contains(key))
                    UnityEngine.Debug.LogWarning("ALERT: OBJECT WAS NULL FOR KEY: " + key);

                return;
            }

            Initialize(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        private void Initialize(object value)
        {
            type = value.GetType().AssemblyQualifiedName;

            this.value = value switch
            {
                int vInt => JsonUtility.ToJson(new Wrapper<int>(vInt)),
                float vFloat => JsonUtility.ToJson(new Wrapper<float>(vFloat)),
                string vString => JsonUtility.ToJson(new Wrapper<string>(vString)),
                bool vBool => JsonUtility.ToJson(new Wrapper<bool>(vBool)),
                IList list => JsonUtility.ToJson(new ListWrapper(list)),
                _ => JsonUtility.ToJson(value)
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            var valueType = Type.GetType(type);

            if (valueType == null)
            {
                valueType = RestoreTypeAttempt();

                if (valueType == null)
                    return null;
            }

            return GetTypeValue(valueType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private Type RestoreTypeAttempt()
        {
            string typeName = FirstSegment(type, ',');

            if (!typeName.Contains(GenericListPrefix) && typeName.Contains('+'))
                typeName = LastSegment(typeName, '+');

            if (string.IsNullOrEmpty(typeName))
                return null;

            Type updatedType = FindTypeByName(typeName);

            if (updatedType == null && typeName.Contains(GenericListPrefix))
            {
                updatedType = GetTypeNameFromList();

                typeName = FirstSegment(typeName.Replace(GenericListPrefix, ""), ',');

                if (typeName.Contains('+'))
                    typeName = LastSegment(typeName, '+');
            }

            if (updatedType == null)
                updatedType = MovedTypes.TryGetValue(typeName, out var t) ? t : null;

            if (updatedType == null)
            {
                UnityEngine.Debug.LogWarning($"ALERT: Failed to restore: {key} : {typeName} : {type}");

                return null;
            }

            type = updatedType.AssemblyQualifiedName;

            return Type.GetType(type);
        }

        /// <summary>
        ///
        /// </summary>
        private static void BuildMovedTypeCache()
        {
            var dataField = typeof(MovedFromAttribute).GetField("data", BindingFlags.NonPublic | BindingFlags.Instance);

            if (dataField == null)
                return;

            foreach (Type t in GetSearchableTypes())
            {
                var movedFrom = t.GetCustomAttribute<MovedFromAttribute>();

                if (movedFrom == null)
                    continue;

                var dataValue = dataField.GetValue(movedFrom);

                if (dataValue == null)
                    continue;

                var classNameField = dataValue.GetType()
                    .GetField("className", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                string sourceClassName = classNameField?.GetValue(dataValue) as string;

                if (!string.IsNullOrEmpty(sourceClassName))
                    MovedTypes.TryAdd(sourceClassName, t);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private Type GetTypeNameFromList()
        {
            string innerTypeRaw = FirstSegment(type.Replace(GenericListPrefix, ""), ',');

            string typeName = LastSegment(innerTypeRaw, '.');

            if (typeName.Contains('+'))
                typeName = LastSegment(typeName, '+');

            Type updatedType = FindTypeByName(typeName);

            if (updatedType == null)
            {
                UnityEngine.Debug.LogWarning($"Could not find List type from {key}: {typeName}");

                return null;
            }

            string updatedGenericTypeString = type.Replace(innerTypeRaw, updatedType.AssemblyQualifiedName);

            Type foundType = Type.GetType(updatedGenericTypeString);

            if (foundType != null)
            {
                string oldPrefix = BeforeLastSegment(innerTypeRaw, '.');

                string newPrefix = BeforeLastSegment(updatedType.FullName, '.');

                if (oldPrefix.Length == 0)
                {
                    UnityEngine.Debug.LogWarning($"Could not find List type from {key}'s because the old prefix was empty, New : {newPrefix}" +
                        $"\nFor Value: {value}");

                    return null;
                }

                value = value.Replace(oldPrefix, newPrefix);
            }

            return foundType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static Type FindTypeByName(string typeName) => TypeNames.TryGetValue(typeName, out var t) ? t : null;

        /// <summary>
        ///
        /// </summary>
        private static void BuildTypeNameCache()
        {
            foreach (var t in GetSearchableTypes())
                TypeNames.TryAdd(t.Name, t);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> GetSearchableTypes() => AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => SearchableAssemblies.Any(filter => assembly.FullName.Contains(filter)))
                .SelectMany(assembly => assembly.GetTypes());

        /// <summary>
        ///
        /// </summary>
        private static string FirstSegment(string input, char separator)
        {
            int index = input.IndexOf(separator);

            return index < 0 ? input : input[..index];
        }

        /// <summary>
        ///
        /// </summary>
        private static string LastSegment(string input, char separator)
        {
            int index = input.LastIndexOf(separator);

            return index < 0 ? input : input[(index + 1)..];
        }

        /// <summary>
        ///
        /// </summary>
        private static string BeforeLastSegment(string input, char separator)
        {
            int index = input.LastIndexOf(separator);

            return index < 0 ? string.Empty : input[..index];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private object GetTypeValue(Type valueType)
        {
            if (IsPrimitiveType(valueType) is object primitiveResult)
                return primitiveResult;

            if (IsGenericType(valueType) is object genericResult)
                return genericResult;

            if (IsArray(valueType) is object arrayResult)
                return arrayResult;

            return DefaultValue(valueType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private object IsPrimitiveType(Type valueType)
        {
            if (valueType == typeof(int))
                return JsonUtility.FromJson<Wrapper<int>>(value).value;

            if (valueType == typeof(float))
                return JsonUtility.FromJson<Wrapper<float>>(value).value;

            if (valueType == typeof(string))
                return JsonUtility.FromJson<Wrapper<string>>(value).value;

            if (valueType == typeof(bool))
                return JsonUtility.FromJson<Wrapper<bool>>(value).value;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private object IsGenericType(Type valueType)
        {
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = valueType.GetGenericArguments()[0];

                var listWrapper = JsonUtility.FromJson<ListWrapper>(value);

                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                foreach (var item in listWrapper.items)
                    if (!string.IsNullOrEmpty(item.value))
                        list.Add(GetElementValue(elementType, item));

                return list;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object GetElementValue(Type elementType, SerializedItem item)
        {
            object elementValue;

            try
            {
                elementValue = elementType.IsPrimitive || elementType == typeof(string)
                ? Convert.ChangeType(item.value, elementType)
                : JsonUtility.FromJson(item.value, elementType);
            }
            catch
            {
#if NEWTONSOFT_JSON
                elementValue = JObjectDeserializeScriptableObject(item.value, elementType);
#else
                return null;
#endif
            }

            return elementValue;
        }
#if NEWTONSOFT_JSON
        /// <summary>
        ///
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static ScriptableObject JObjectDeserializeScriptableObject(string json, Type type)
        {
            var instance = ScriptableObject.CreateInstance(type);

            JsonConvert.PopulateObject(json, instance);

            return instance;
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private object IsArray(Type valueType)
        {
            if (valueType.IsArray)
            {
                var elementType = valueType.GetElementType();

                var listWrapper = JsonUtility.FromJson<ListWrapper>(value);

                var array = Array.CreateInstance(elementType, listWrapper.items.Count);

                for (int i = 0; i < listWrapper.items.Count; i++)
                {
                    var item = listWrapper.items[i];

                    if (string.IsNullOrEmpty(item.value))
                        continue;

                    array.SetValue(GetElementValue(elementType, item), i);
                }

                return array;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private object DefaultValue(Type valueType)
        {
            try
            {
                if (valueType.IsGenericType)
                {
                    UnityEngine.Debug.Log($"REPORT: Entry Key: {key}, with type: {type} was found to be surface level generic." +
                        $" ");

                    return null;
                }

                if (typeof(MonoBehaviour).IsAssignableFrom(valueType))
                {
                    UnityEngine.Debug.Log($"REPORT: {valueType} was {nameof(MonoBehaviour)}, this is not allowed, " +
                        $"and restoring this data will be skipped. " +
                        $"\nHYPOTHESIS: It is likely that the original data was renamed to something else, " +
                        $"and a class that inherits {nameof(MonoBehaviour)} is now using the type name instead." +
                        $"\nPROPOSAL: If this is the case, ensure that the data class that this data should have been restored to uses " +
                        $"unity's MovedFrom Attribute in your code in order to restore this corrupted data.");

                    return null;
                }

                return JsonUtility.FromJson(value, valueType);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"ALERT: COULDNT GET TYPE FOR {key} : {type} : {value} " +
                    $"\nREASON: {e}");

                return null;
            }
        }
    }
}