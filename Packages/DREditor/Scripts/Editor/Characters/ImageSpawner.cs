//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Editor.Characters
{
    /// <summary>
    ///
    /// </summary>
    public static class ImageSpawner
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/UI/Implement Images to Selected Canvas")]
        public static void ImplementImages()
        {
            foreach(var gameObject in Selection.gameObjects)
                if (gameObject.GetComponent<Canvas>() is Canvas canvas && canvas != null)
                    foreach (var obj in Selection.objects)
                        if (SpawnSpriteOrContinue(obj, canvas))
                            continue;

            UnityEngine.Debug.Log("Finished");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        static bool SpawnSpriteOrContinue(Object obj, Canvas canvas)
        {
            if (obj is GameObject)
                return true;

            if (obj is Sprite s)
            {
                UnityEngine.Debug.Log("Found: " + obj.name);

                MakeImageObject(s, canvas.gameObject.transform);
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        static GameObject MakeImageObject(Sprite sprite, Transform parent)
        {
            GameObject gameObject = new(sprite.name, components: typeof(Image));

            gameObject.GetComponent<Image>().sprite = sprite;

            gameObject.transform.parent = parent;

            gameObject.transform.localPosition = Vector3.zero;

            gameObject.GetComponent<Image>().SetNativeSize();

            return gameObject;
        }
    }
}
#endif