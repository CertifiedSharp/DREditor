//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Talkables;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DREditor.Actors.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class ItemUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Items/Apply Item Properties")]
        public static void ApplyItem()
        {
            foreach (GameObject gameObject in Selection.gameObjects)
                ApplyItemProperties(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="addCamsToBodies"></param>
        public static void ApplyItemProperties(GameObject gameObject, bool addCamsToBodies = false)
        {
            ItemActor item = gameObject.AddComponent<ItemActor>();

            while (UnityEditorInternal.ComponentUtility.MoveComponentUp(item)) { }

            item.EditorInstantiation();

            if (addCamsToBodies && gameObject.FindFirstChildContaining("Body", out GameObject child))
                child.AddComponent<UnityEngine.Camera>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="subString"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool FindFirstChildContaining(this GameObject gameObject, string subString, out GameObject result)
        {
            result = null;

            foreach (Transform child in gameObject.transform)
                if (child.name.Contains(subString))
                    result = child.gameObject;

            return result != null;
        }


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Items/Remove Cameras")]
        public static void RemoveCameras()
        {
            var gameObjects = MonoBehaviour
                .FindObjectsByType<GameObject>()
                .Where(gameObject =>
                !gameObject.name.Contains("Camera") &&
                gameObject.GetComponent<UnityEngine.Camera>() != null &&
                gameObject.activeSelf)
                .ToList();

            foreach (GameObject gameObject in gameObjects)
            {
                MonoBehaviour.DestroyImmediate(gameObject.GetComponent<UniversalAdditionalCameraData>());

                MonoBehaviour.DestroyImmediate(gameObject.GetComponent<UnityEngine.Camera>());

                EditorUtility.SetDirty(gameObject);
            }
        }
    }
}
