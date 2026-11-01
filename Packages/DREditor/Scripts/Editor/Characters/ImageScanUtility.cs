//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Editor.Characters
{
    /// <summary>
    ///
    /// </summary>
    public static class ImageScanUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/Image Objects for Dimension")]
        public static void ScanImageObjectsForDimension()
        {
            string imageList = "";

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                RawImage rawImage = gameObject.GetComponent<RawImage>();

                Image image = gameObject.GetComponent<Image>();

                if (image != null || (rawImage != null && rawImage.texture is not RenderTexture))
                {
                    imageList += gameObject.name +
                        "(" +
                        (image != null ? image.rectTransform.sizeDelta.x : rawImage.rectTransform.sizeDelta.x) +
                        " x " +
                        (image != null ? image.rectTransform.sizeDelta.y : rawImage.rectTransform.sizeDelta.y) +
                        ")";

                    imageList += "\n";
                }
            }

            GUIUtility.systemCopyBuffer = imageList;
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/Folder for Incorrect Dimensions")]
        public static void ScanForIncorrectDimensions()
        {
            if (Selection.objects.Length == 0 || !FolderUtility.SelectedObjectIsFolder())
            {
                UnityEngine.Debug.LogWarning("ALERT: To use this tool you must select a folder. Aborting Process.");

                return;
            }

            Object selectedObject = Selection.activeObject;

            string assetPath = AssetDatabase.GetAssetPath(selectedObject);

            string[] folder = new string[1];

            folder[0] = assetPath;

            UnityEngine.Debug.Log("Scanning for Textures not divisible by 4: Starting Scan.");

            string imageList = "";

            var textures = BuilderUtility.FindAssetsByType<Texture2D>("t:Texture2D", folder);

            int count = 0;

            int maxCount = 100;

            foreach (var texture in textures)
            {
                if (count > maxCount)
                    break;

                if (!texture.DimensionsDivisibleByFour())
                {
                    imageList += texture.name + " (" + texture.width + " x " + texture.height + ")";

                    imageList += "\n";
                }

                count++;
            }

            GUIUtility.systemCopyBuffer = imageList;

            UnityEngine.Debug.Log("Scanning for Textures not divisible by 4: Scan Complete.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static bool DimensionsDivisibleByFour(this Texture texture) => texture.width % 4 == 0 && texture.height % 4 == 0;
    }
}