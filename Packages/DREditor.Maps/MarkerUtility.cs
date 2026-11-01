//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public static class MarkerUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Import/Map/Markers")]
        public static void ImportImage()
        {
            foreach(var selectedObject in Selection.objects)
                if (selectedObject is Sprite sprite)
                    ImportImageMarker(sprite);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        static void ImportImageMarker(Sprite sprite)
        {
            MarkerVessel data = ScriptableObject.CreateInstance<MarkerVessel>();

            data.name = sprite.name;

            data.markerData.SetName(sprite.name);

            data.SetTexture(sprite);

            string path = AssetDatabase.GetAssetPath(sprite);

            path = path.Remove(path.LastIndexOf("/") + 1);

            path += sprite.name + ".asset";

            AssetDatabase.CreateAsset(data, path);
        }
    }
}
#endif