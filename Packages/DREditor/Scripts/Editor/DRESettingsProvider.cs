//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public static class DRESettingsProvider
    {
        /// <summary>
        ///
        /// </summary>
        private const string InitializeMethodName = "Initialize";

        /// <summary>
        ///
        /// </summary>
        private const string CreateProviderMethodName = "CreateSettingsProvider";


        static DRESettingsProvider() => InitializeProviderTypes();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [SettingsProviderGroup]
        public static SettingsProvider[] CreateAllProviders()
        {
            var providers = new List<SettingsProvider>();

            InitializeProviderTypes(providers);

            return providers.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="providers"></param>
        static void InitializeProviderTypes(List<SettingsProvider> providers = null)
        {
            var settingTypes = TypeCache
                .GetTypesDerivedFrom(typeof(ProjectSettingsData<>))
                .Where(t => !t.IsAbstract);

            foreach (var type in settingTypes)
                if (!InitializeProviderType(type, providers))
                    continue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="providers"></param>
        /// <returns></returns>
        static bool InitializeProviderType(System.Type type, List<SettingsProvider> providers = null)
        {
            var baseType = type.BaseType;

            if (baseType == null || !baseType.IsGenericType)
                return false;

            var wrapperType = typeof(SettingsProviderBase<>).MakeGenericType(type);

            wrapperType.GetMethod(InitializeMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                ?.Invoke(null, null);

            if (providers != null &&
                    wrapperType
                    .GetMethod(CreateProviderMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)?
                    .Invoke(null, null) is SettingsProvider provider)
                providers.Add(provider);

            return true;
        }
    }
}