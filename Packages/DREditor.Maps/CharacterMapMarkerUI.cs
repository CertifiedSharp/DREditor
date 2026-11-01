//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class CharacterMapMarkerUI : MonoBehaviour, IEntryLoader
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float separationValue = 50f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 markerScale = new(2, 2, 2);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> characterNames = new();


        /// <inheritdoc/>
        public virtual void LoadEntry(object entry, List<Image> markers) => ArrangeMarkers(GetCharacterMarkers(markers));

        /// <summary>
        ///
        /// </summary>
        /// <param name="markers"></param>
        /// <returns></returns>
        protected virtual List<Image> GetCharacterMarkers(List<Image> markers)
        {
            List<Image> charMarkers = new();

            foreach (var item in markers)
                if (characterNames.Contains(item.gameObject.name))
                    charMarkers.Add(item);

            return charMarkers;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="charMarkers"></param>
        protected virtual void ArrangeMarkers(List<Image> charMarkers)
        {
            int count = charMarkers.Count;

            if (count == 0)
                return;

            int gridSize = Mathf.CeilToInt(Mathf.Sqrt(count));

            int index = 0;

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize && index < count; x++)
                {
                    float offsetX = (x - (gridSize - 1) / 2f) * separationValue * 2;

                    float offsetY = (y - (gridSize - 1) / 2f) * separationValue * 2;

                    SetScalePosition(charMarkers[index].transform, offsetX, offsetY);

                    index++;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected virtual void SetScalePosition(Transform transform, float x, float y)
        {
            transform.localPosition = new Vector3(x, y, 0);

            transform.localScale = markerScale;
        }
    }
}