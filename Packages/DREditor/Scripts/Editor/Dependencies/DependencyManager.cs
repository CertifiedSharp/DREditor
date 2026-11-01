//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    public static class DependencyManager
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly string AllDependenciesInstalledKey = "AllDependenciesInstalled";


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [MenuItem("DREditor/Dependencies/Reset Dependencies", true, priority = 1)]
        public static bool AllDependenciesInstalled() => EditorPrefs.GetBool(AllDependenciesInstalledKey);

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Dependencies/Reset Dependencies", false, priority = 1)]
        public static void ResetDependencies()
        {
            EditorPrefs.DeleteAll();

            CompilationPipeline.RequestScriptCompilation();
        }

        /// <summary>
        ///
        /// </summary>
        public static void InstallDependencies(List<DependencyInitializerBase> dependencies)
        {
            if (!EditorPrefs.HasKey(AllDependenciesInstalledKey))
                dependencies.ForEach(dependency => dependency.ClearIgnore());

            EditorPrefs.SetBool(AllDependenciesInstalledKey, TryInitializeDependencies(dependencies));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        private static bool TryInitializeDependencies(List<DependencyInitializerBase> dependencies)
        {
            foreach (var type in dependencies)
                if (!type.TryToComplete() && type.Required)
                    return false;

            return true;
        }
    }
}