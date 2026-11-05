//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Editor.Characters
{
    /// <summary>
    /// Utility class for scanning images and textures in a DREditor Project.
    /// </summary>
    public static class ImageScanUtility
    {
        /// <summary>
        /// Scans a parent game object and it's children for the image component and then outputting the dimension size on the COMPONENT.
        /// </summary>
        [MenuItem("DREditor/Utility/Scan/Image Objects for Dimension")]
        public static void ScanImageObjectsForDimension()
        {
            string foundImages = "";

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                RawImage rawImage = gameObject.GetComponent<RawImage>();

                Image image = gameObject.GetComponent<Image>();

                if (image != null || (rawImage != null && rawImage.texture is not RenderTexture))
                {
                    foundImages += gameObject.name +
                        "(" +
                        (image != null ? image.rectTransform.sizeDelta.x : rawImage.rectTransform.sizeDelta.x) +
                        " x " +
                        (image != null ? image.rectTransform.sizeDelta.y : rawImage.rectTransform.sizeDelta.y) +
                        ")";

                    foundImages += "\n";
                }
            }

            GUIUtility.systemCopyBuffer = foundImages;
        }

        /// <summary>
        /// Scans a selected folder's texture's width and height to see if they're are divisible by four. To signify a properly imported texture.
        /// </summary>
        [MenuItem("Assets/DREditor/Scan Folder for Incorrect Dimensions")]
        public static void ScanForIncorrectDimensions()
        {
            if (!FolderUtility.TryGetSelectedFolderPath(out string assetPath))
            {
                UnityEngine.Debug.LogWarning("ALERT: To use this tool you must select a folder. Aborting Process.");

                return;
            }

            string[] folder = new string[1];

            folder[0] = assetPath;

            UnityEngine.Debug.Log($"REPORT: Scanning {assetPath} for Textures not divisible by 4: Starting Scan.");

            string foundTextures = "";

            var textures = BuilderUtility.FindAssetsByType<Texture2D>("t:Texture2D", folder);

            int count = 0;

            int maxCount = 100;

            foreach (var texture in textures)
            {
                if (count >= maxCount || texture.DimensionsDivisibleByFour())
                    continue;

                string foundTexture = texture.name + " (" + texture.width + " x " + texture.height + ")";

                if (count <= maxCount)
                    UnityEngine.Debug.Log("REPORT: Found: " + foundTexture);

                foundTextures += foundTexture + "\n";

                count++;
            }

            if (count >= maxCount)
                UnityEngine.Debug.LogWarning($"ALERT: " +
                    $"The maximum result limit of {maxCount} has been reached to protect the console and clipboard. Exiting early.");

            GUIUtility.systemCopyBuffer = foundTextures;

            UnityEngine.Debug.Log("REPORT: Scanning Complete. Copying results to clipboard.");
        }

        /// <summary>
        /// Extension method to check whether a texture's width and height are divisible by four. To signify a properly imported texture.
        /// </summary>
        /// <param name="texture">Texture to be evaluated.</param>
        /// <returns>True if the texture's width and height are divisible by four.</returns>
        public static bool DimensionsDivisibleByFour(this Texture texture) => texture.width % 4 == 0 && texture.height % 4 == 0;
    }
}