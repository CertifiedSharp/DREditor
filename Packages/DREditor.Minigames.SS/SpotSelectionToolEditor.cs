//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class SpotSelectionToolEditor : SpotSelectionEditorComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        protected Canvas canvas;

        /// <summary>
        ///
        /// </summary>
        protected Image cg;


        public SpotSelectionToolEditor(SpotSelectionBuilder builder) : base(builder) { }


        /// <inheritdoc/>
        public override void Display()
        {
            GUILayout.Label("Testing Tools", GUILayout.Width(50));

            DisplayCanvasTool();

            DisplayImageTool();

            GUILayout.Space(30);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCanvasTool()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Test Canvas: ", GUILayout.Width(90));

                builder.SetTestCanvas(EditorGUILayout.TextField(builder.testCanvas, GUILayout.Width(125)));

                DisplayExpotSpotsButton();

                DisplayClearButton();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayExpotSpotsButton()
        {
            if (GUILayout.Button("Export Spots", GUILayout.Width(100)))
                for (int i = 0; i < builder.spots.Count; i++)
                    ExportSpot(builder.spots[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spot"></param>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void ExportSpot(Spot spot)
        {
            if (canvas == null && GameObject.Find(builder.testCanvas).TryGetComponent<Canvas>(out canvas))
            {
                GameObject spotGameObject = new(spot.spotName);

                spotGameObject.transform.parent = canvas.transform;

                BoxCollider2D collider = spotGameObject.AddComponent<BoxCollider2D>();

                collider.size = spot.size;

                collider.offset = spot.center;

                spotGameObject.transform.position = spot.position;

                spotGameObject.transform.eulerAngles = spot.rotation;
            }
            else
                UnityEngine.Debug.LogWarning("The Canvas by that name was not found in the scene!");
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayClearButton()
        {
            if (GUILayout.Button("Clear Canvas Spots", GUILayout.Width(150)) &&
                    EditorUtility.DisplayDialog(
                        "Clear Canvas Spots",
                        "Are you sure you want to Clear All Canvas Spots?",
                        "Yes", "No"))
                ClearDebugUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearDebugUI()
        {
            if (canvas == null && GameObject.Find(builder.testCanvas).TryGetComponent<Canvas>(out canvas))
            {
                ClearSpotColliders();

                ClearDisplayImage();
            }
            else
                UnityEngine.Debug.LogWarning("The Canvas by that name was not found in the scene!");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearSpotColliders()
        {
            BoxCollider2D[] boxes = canvas.GetComponentsInChildren<BoxCollider2D>();

            for (int i = 0; i < boxes.Length; i++)
                if (boxes[i].GetComponent<Rigidbody2D>() != null)
                    continue;
                else
                    MonoBehaviour.DestroyImmediate(boxes[i].gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearDisplayImage()
        {
            Image image = GameObject.Find(builder.testCG).GetComponent<Image>();

            image.sprite = null;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayImageTool()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Test CG: ", GUILayout.Width(90));

                builder.SetTestCG(EditorGUILayout.TextField(builder.testCG, GUILayout.Width(125)));

                if (GUILayout.Button("Set CG", GUILayout.Width(100)))
                {
                    cg = GameObject.Find(builder.testCG).GetComponent<Image>();

                    cg.sprite = builder.sprite;
                }
            }
        }
    }
}
#endif