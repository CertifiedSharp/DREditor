//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public static class DRETemplateUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Must be Checked in order because "ETemplate" also ends with "Template".</remarks>
        private static readonly string[] TemplateSuffixes = { "ETemplate", "Template", "Shell" };

        /// <summary>
        /// 
        /// </summary>
        private static readonly char TemplateNameSeparator = '_';


        /// <summary>
        /// 
        /// </summary>
        public static GameObject GetSelectedPrefab()
        {
            GameObject gameObject = Selection.activeGameObject;

            if (Selection.gameObjects.Length != 1 ||
                SelectedProjectWindowAsset(gameObject) ||
                PrefabInvalid(gameObject))
                return null;

            return PrefabUtility.GetCorrespondingObjectFromSource(gameObject) != null ? gameObject : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private static bool SelectedProjectWindowAsset(GameObject gameObject) =>
            gameObject == null || EditorUtility.IsPersistent(gameObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private static bool PrefabInvalid(GameObject gameObject) =>
            !PrefabUtility.IsAnyPrefabInstanceRoot(gameObject) || PrefabUtility.IsPrefabAssetMissing(gameObject);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentPath"></param>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static bool AssetExistsAt(string sentPath, out string assetPath)
        {
            assetPath = sentPath;

            bool alreadyExists = File.Exists(sentPath) || AssetDatabase.LoadMainAssetAtPath(sentPath) != null;

            return alreadyExists && !UniquePathPrompt(sentPath, out assetPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentPath"></param>
        /// <param name="unique"></param>
        /// <returns></returns>
        private static bool UniquePathPrompt(string sentPath, out string unique)
        {
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(sentPath);

            bool useUnique = EditorUtility.DisplayDialog(
                "Prefab already exists",
                $"ALERT: '{sentPath}' already exists." +
                $"\n\nPROPOSAL: Create '{Path.GetFileName(uniquePath)}' instead?",
                "Use Unique Name", "Cancel");

            unique = useUnique ? uniquePath : sentPath;

            return useUnique;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceRoot"></param>
        /// <param name="variantAssetPath"></param>
        public static void CreateProjectPrefabVariant(GameObject instanceRoot, string variantAssetPath)
        {
            int undoGroup = Undo.GetCurrentGroup();

            if (!TryMakeProjectPrefabVariant(instanceRoot, variantAssetPath, out GameObject variantAsset))
                return;

            RenameHierarchyObjectToVariantName(instanceRoot, variantAssetPath);

            Undo.CollapseUndoOperations(undoGroup);

            FlagSceneOrOpenedPrefabAsChanged(instanceRoot);

            EditorGUIUtility.PingObject(variantAsset);

            Debug.Log($"REPORT: Created Project Prefab Variant at: '{variantAssetPath}'.", variantAsset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceRoot"></param>
        /// <param name="variantAssetPath"></param>
        /// <param name="variantAsset"></param>
        /// <returns></returns>
        private static bool TryMakeProjectPrefabVariant(GameObject instanceRoot, string variantAssetPath, out GameObject variantAsset)
        {
            variantAsset = null;

            bool succeded = false;

            try
            {
                variantAsset = PrefabUtility.SaveAsPrefabAssetAndConnect(instanceRoot, variantAssetPath, InteractionMode.UserAction,
                    out succeded);
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"ALERT: Create Prefab Variant failed!");

                Debug.LogException(exception);
            }

            if (!succeded || variantAsset == null)
                Debug.LogError($"ALERT: Create Prefab Variant: could not create a Prefab Variant at '{variantAssetPath}'.");

            return succeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceRoot"></param>
        /// <param name="variantAssetPath"></param>
        private static void RenameHierarchyObjectToVariantName(GameObject instanceRoot, string variantAssetPath)
        {
            string variantName = Path.GetFileNameWithoutExtension(variantAssetPath);

            if (instanceRoot.name != variantName)
            {
                Undo.RecordObject(instanceRoot, "Rename Prefab Variant Instance");

                instanceRoot.name = variantName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instanceRoot"></param>
        private static void FlagSceneOrOpenedPrefabAsChanged(GameObject instanceRoot)
        {
            PrefabStage stage = PrefabStageUtility.GetPrefabStage(instanceRoot);

            if (stage != null)
                EditorSceneManager.MarkSceneDirty(stage.scene);
            else if (instanceRoot.scene.IsValid())
                EditorSceneManager.MarkSceneDirty(instanceRoot.scene);
            else
                Debug.LogWarning("ALERT: Somehow, there is no area in which to set what's currently happening as changed.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceAsset"></param>
        /// <param name="suffixText"></param>
        /// <returns></returns>
        public static string CreateProjectVariantName(GameObject sourceAsset, string suffixText) =>
            CreateProjectVariantName(GetPrefabName(sourceAsset), suffixText);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefabAsset"></param>
        /// <returns></returns>
        public static string GetPrefabName(GameObject prefabAsset) =>
            Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(prefabAsset));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="suffixText"></param>
        /// <returns></returns>
        private static string CreateProjectVariantName(string templateName, string suffixText)
        {
            string templateBaseName = ReplaceTemplateName(templateName);

            string projectSuffix = TrimSuffix(templateBaseName, suffixText);

            return templateBaseName + projectSuffix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateBaseName"></param>
        /// <param name="suffixText"></param>
        /// <returns></returns>
        private static string TrimSuffix(string templateBaseName, string suffixText) =>
            templateBaseName.EndsWith(TemplateNameSeparator) ? suffixText.TrimStart(TemplateNameSeparator) : suffixText;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="replaceName"></param>
        /// <returns></returns>
        private static string ReplaceTemplateName(string replaceName)
        {
            string templateSuffix = replaceName.GetTemplateSuffix();

            bool shouldReplacedTemplate = !string.IsNullOrEmpty(templateSuffix);

            if (shouldReplacedTemplate)
                replaceName = replaceName[..^templateSuffix.Length];

            if (!shouldReplacedTemplate && !replaceName.EndsWith(TemplateNameSeparator))
                replaceName += TemplateNameSeparator;

            return replaceName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private static string GetTemplateSuffix(this string objectName)
        {
            foreach (string ending in TemplateSuffixes)
                if (objectName.EndsWith(ending, System.StringComparison.Ordinal))
                    return ending;

            return string.Empty;
        }
    }
}