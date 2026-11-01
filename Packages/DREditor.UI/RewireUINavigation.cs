//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public static class RewireUINavigation
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectables"></param>
        public static void PopulateVerticalNavigation<T>(this List<T> selectables) where T : Selectable
        {
            for (int i = 0; i < selectables.Count; i++)
                if (selectables[i].navigation.selectOnDown != null)
                    break;
                else
                    SetVerticalNavigation(selectables, i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectables"></param>
        /// <param name="i"></param>
        public static void SetVerticalNavigation<T>(List<T> selectables, int i) where T : Selectable
        {
            if (i == 0)
                selectables[i].SelectOnDown(selectables[i + 1]);
            else if (i == selectables.Count - 1)
                selectables[i].SelectOnUp(selectables[i - 1]);
            else
                selectables[i].SelectUpDown(selectables[i - 1], selectables[i + 1]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectables"></param>
        /// <param name="newSelectable"></param>
        /// <param name="direction"></param>
        public static void ChangeGroupNav<T>(this List<T> selectables, Selectable newSelectable, Direction direction) where T : Selectable
        {
            foreach (T selectable in selectables)
                selectable.ChangeNavSelects(newSelectable, direction);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="selectableOnDirection"></param>
        /// <param name="direction"></param>
        public static void ChangeNavSelects(this Selectable selectable, Selectable selectableOnDirection, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    selectable.SelectOnUp(selectableOnDirection);
                    break;
                case Direction.Down:
                    selectable.SelectOnDown(selectableOnDirection);
                    break;
                case Direction.Left:
                    selectable.SelectOnLeft(selectableOnDirection);
                    break;
                case Direction.Right:
                    selectable.SelectOnRight(selectableOnDirection);
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        public static void SelectUpDown(this Selectable on, Selectable up, Selectable down)
        {
            on.SelectOnUp(up);

            on.SelectOnDown(down);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        /// <param name="to"></param>
        public static void SelectOnUp(this Selectable on, Selectable to)
        {
            Navigation n = on.navigation;

            n.selectOnUp = to;

            on.navigation = n;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        /// <param name="to"></param>
        public static void SelectOnDown(this Selectable on, Selectable to)
        {
            Navigation n = on.navigation;

            n.selectOnDown = to;

            on.navigation = n;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        /// <param name="to"></param>
        public static void SelectOnLeft(this Selectable on, Selectable to)
        {
            Navigation n = on.navigation;

            n.selectOnLeft = to;

            on.navigation = n;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="on"></param>
        /// <param name="to"></param>
        public static void SelectOnRight(this Selectable on, Selectable to)
        {
            Navigation n = on.navigation;

            n.selectOnRight = to;

            on.navigation = n;
        }

        /// <summary>
        ///
        /// </summary>
        private static readonly Vector3[] Directions = new Vector3[]
        {
            Vector3.up,
            Vector3.right,
            Vector3.down,
            Vector3.left
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="directionThreshold"></param>
        /// <returns>
        ///
        /// </returns>
        public static List<Transform> FindClosestTransforms(Transform currentTransform, float directionThreshold = 0.8f)
        {
            Transform parentTransform = currentTransform.parent;

            if (parentTransform == null)
            {
                UnityEngine.Debug.LogWarning($"ALERT: Transform '{currentTransform.name}' has no parent. Cannot search.");

                return new List<Transform> { null, null, null, null };
            }

            List<Transform> results = new(4);

            for (int i = 0; i < Directions.Length; i++)
            {
                Vector3 direction = Directions[i];

                Transform closestMatch = null;

                float closestSqrDistance = float.MaxValue;

                foreach (Transform sibling in parentTransform)
                {
                    if (sibling == currentTransform)
                        continue;

                    Vector3 localDifference = sibling.localPosition - currentTransform.localPosition;

                    float dotProduct = Vector3.Dot(localDifference.normalized, direction);

                    if (dotProduct > directionThreshold)
                    {
                        float sqrDistance = localDifference.sqrMagnitude;

                        if (sqrDistance < closestSqrDistance)
                        {
                            closestSqrDistance = sqrDistance;

                            closestMatch = sibling;
                        }
                    }
                }

                results.Add(closestMatch);
            }

            return results;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum Direction
    {
        Up, Down, Left, Right
    }
}