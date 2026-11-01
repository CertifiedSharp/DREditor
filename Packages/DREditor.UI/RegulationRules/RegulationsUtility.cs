//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    public static class RegulationsUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Import/Regulations")]
        public static void ImportRegulations()
        {
            UnityEngine.Debug.Log("This takes selected sprites in the project window and creates regulation assets, " +
                "then takes copied regulation text from a document " +
                "and applies it to the created regulations.");

            List<Regulation> regulations = CreateRegulationsFromSpritesOrText();

            ApplyCopiedTextToRegulations(regulations);

            foreach (var regulation in regulations)
                EditorUtility.SetDirty(regulation);

            AddRegulationsToDatabase(regulations);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="regulations"></param>
        static void AddRegulationsToDatabase(List<Regulation> regulations)
        {
            RegulationsDatabase database = DatabaseUtility.LoadDatabase<RegulationsDatabase>() == null ?
                CustomEditorUtility.CreateAssetWithFolders<RegulationsDatabase>("RegulationsDatabase", "Assets/Resources/Regulations/") :
                DatabaseUtility.LoadDatabase<RegulationsDatabase>();

            foreach (Regulation regulation in regulations)
                database.AddRegulation(regulation);

            EditorUtility.SetDirty(database);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        static List<Regulation> CreateRegulationsFromSpritesOrText()
        {
            List<Regulation> regulations = new();

            int offsetNumber = 1;

            if (DatabaseUtility.LoadDatabase<RegulationsDatabase>() is RegulationsDatabase regulationsDatabase)
            {
                regulationsDatabase.ClearEmptyValues();

                int regulationsCount = regulationsDatabase.GetRegulations().Count;

                offsetNumber = regulationsCount == 0 ? 1 : regulationsCount;
            }

            var sprites = GetSelectedObjectsOfType<Sprite>();

            string[] regulationTexts = EditorGUIUtility.systemCopyBuffer.Split('\n', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(splitText => splitText.Trim())
                .Where(splitText => splitText.Length > 0)
                .ToArray();

            int count = sprites.Count != 0 ? sprites.Count : regulationTexts.Length;

            for (int i = 0; i < count; i++)
            {
                var regulation = CustomEditorUtility.CreateAssetWithFolders<Regulation>("Regulation " + (i + offsetNumber).ToString("D2"));

                if (i < sprites.Count)
                {
                    regulation.SetImage(sprites[i]);

                    EditorUtility.SetDirty(regulation);

                    AssetDatabase.SaveAssets();

                    AssetDatabase.Refresh();
                }

                regulations.Add(regulation);
            }

            return regulations;
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Regulations/Paste Regulation Text")]
        public static void PasteRegulations()
        {
            UnityEngine.Debug.Log("This tool takes copied regulation text from a document " +
                "and applies it to regulations selected in the project window.");

            ApplyCopiedTextToRegulations(GetSelectedObjectsOfType<Regulation>());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="regulations"></param>
        static void ApplyCopiedTextToRegulations(List<Regulation> regulations)
        {
            string[] regulationTexts = EditorGUIUtility.systemCopyBuffer.Split('\n', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(splitText => splitText.Trim())
                .Where(splitText => splitText.Length > 0)
                .ToArray();

            for (int i = 0; i < regulationTexts.Length && i < regulations.Count; i++)
            {
                UnityEngine.Debug.Log("Adding regulation text to: " + regulations[i].name + " with: " + regulationTexts[i]);

                regulations[i].SetText(regulationTexts[i]);

                EditorUtility.SetDirty(regulations[i]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static List<T> GetSelectedObjectsOfType<T>()
        {
            List<T> objects = new();

            foreach (var obj in Selection.objects)
                if (obj is T objectOfType)
                    objects.Add(objectOfType);

            return objects;
        }
    }
}
#endif