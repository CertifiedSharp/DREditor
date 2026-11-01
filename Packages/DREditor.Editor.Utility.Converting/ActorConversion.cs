//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.EventObjects;
using DREditor.Processables;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility.Converting
{
    /// <summary>
    ///
    /// </summary>
    public static class ActorConversion
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Convert/Actors/Update Selected Actor Display Names.")]
        public static void UpdateName()
        {
            CharacterDatabase database = DatabaseUtility.LoadDatabase<CharacterDatabase>();

            foreach (Character character in database.Characters)
                foreach (var obj in Selection.objects)
                    if (obj is GameObject gameObject && gameObject.name == character.FirstName)
                        UpdateObject(character, gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="go"></param>
        static void UpdateObject(Character character, GameObject go)
        {
            if (go.TryGetComponent<INameDisplayable>(out var component))
                component.DisplayName = character.FirstName + " " + character.LastName;

            EditorUtility.SetDirty(go);

            AssetDatabase.SaveAssets();
        }


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Convert/Actors/Update Selected Actor Colliders.")]
        public static void UpdateColliders()
        {
            foreach (var obj in Selection.objects)
                if (obj is GameObject go)
                    UpdateCollider(go);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObject"></param>
        static void UpdateCollider(GameObject gameObject)
        {
            BoxCollider parentCollider = gameObject.GetComponent<BoxCollider>();

            BoxCollider childCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider>();

            if (!parentCollider || !childCollider || parentCollider == childCollider)
            {
                UnityEngine.Debug.LogWarning("ALERT: Invalid collider setup. Either your missing a parent or child collider, or " +
                    "the child and parent colliders are the same, which is not allowed.");

                return;
            }

            Undo.RecordObject(parentCollider, "Copy Collider From Child (Apply Scale)");

            Transform parentTransform = parentCollider.transform;

            Transform childTransform = childCollider.transform;

            var bounds = new Bounds(
                childTransform.TransformPoint(childCollider.center),
                Vector3.Scale(childCollider.size, childTransform.lossyScale)
            );

            Vector3 localCenter = parentTransform.InverseTransformPoint(bounds.center);

            Vector3 localSize = Vector3.Scale(bounds.size, InverseScale(parentTransform.lossyScale));

            parentCollider.center = localCenter;

            parentCollider.size = localSize;

            EditorUtility.SetDirty(parentCollider);

            UnityEngine.Debug.Log($"Copied scaled collider from '{childCollider.name}' to '{parentCollider.name}'.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        static Vector3 InverseScale(Vector3 vector) => new(1f / vector.x, 1f / vector.y, 1f / vector.z);
    }
}