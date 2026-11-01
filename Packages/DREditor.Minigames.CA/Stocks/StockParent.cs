// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.CA.Stocks
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class StockParent : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject stockPrefab = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string indexNameDivider = "_";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 rotation = new(0, 0, -8);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField]
        protected List<Vector3> stockPositions = new()
        {
            new(10.88f, -2.59f, 0f),
            new(11.92f, -2.56f, 0f),
            new(11.02f, -1.63f, 0f),
            new(12.06f, -1.60f, 0f),
            new(10.74f, -3.55f, 0f),
            new(11.78f, -3.52f, 0f),
            new(11.16f, -0.67f, 0f),
            new(12.20f, -0.64f, 0f),
            new(10.60f, -4.51f, 0f),
            new(11.64f, -4.48f, 0f)
        };


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Record")]
        public void Record()
        {
            stockPositions.Clear();

            int stockCount = GetComponentsInChildren<ClosingArgumentStockBase>().Length;

            for (int i = 0; i < stockCount; i++)
            {
                ClosingArgumentStockBase currentStock = transform.GetChild(i).GetComponent<ClosingArgumentStockBase>();

                stockPositions.Add(currentStock.transform.localPosition);
            }
        }


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Apply")]
        public void Apply()
        {
            DestroyAllChildren();

#if UNITY_EDITOR

            for (int i = 0; i < stockPositions.Count; i++)
            {
                var instance = UnityEditor.PrefabUtility.InstantiatePrefab(stockPrefab, transform);

                instance.name = instance.name + indexNameDivider + i;
            }

#endif

            for (int i = 0; i < stockPositions.Count; i++)
            {
                ClosingArgumentStockBase currentStock = transform.GetChild(i).GetComponent<ClosingArgumentStockBase>();

                currentStock.transform.localPosition = stockPositions[i];

                currentStock.transform.localEulerAngles = rotation;
            }

            var stocks = GetComponentsInChildren<ClosingArgumentStockBase>();

            foreach (var stock in stocks)
                stock.Rewire();

#if UNITY_EDITOR

            EditorUtility.SetDirty(transform.root);

#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected void DestroyAllChildren()
        {
            int childCount = transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
                if (Application.isPlaying)
                    Destroy(transform.GetChild(i).gameObject);
                else
                    DestroyImmediate(transform.GetChild(i).gameObject);

            UnityEngine.Debug.Log($"{childCount} children destroyed from parent: {gameObject.name}");
        }
    }
}