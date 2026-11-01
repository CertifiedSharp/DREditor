//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class SpotsEditor : SpotSelectionEditorComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        protected Spot spot;


        public SpotsEditor(SpotSelectionBuilder builder) : base(builder) { }


        /// <inheritdoc/>
        public override void Display()
        {
            for (int i = 0; i < builder.spots.Count; i++)
            {
                SetCurrentSpot(builder.spots[i]);

                DisplaySpot();

                DisplayRemoveSpotButton(i);
            }

            DisplayAddSpotButton();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spot"></param>
        protected virtual void SetCurrentSpot(Spot spot) => this.spot = spot;


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpot()
        {
            using (new GUILayout.HorizontalScope())
            {
                DisplaySpotName();

                DisplayImportSpotButton();

                DisplayClearSpotButton();
            }

            DisplaySpotAnswer();

            DisplaySpotWrongDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpotName()
        {
            GUILayout.Label("Name: ", GUILayout.Width(40));

            spot.SetSpotName(EditorGUILayout.TextField(spot.spotName, GUILayout.Width(125)));

            GUILayout.Space(10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayImportSpotButton()
        {
            if (GUILayout.Button("Import Spot", GUILayout.Width(100)) &&
                    EditorUtility.DisplayDialog(
                        "Import Spot",
                        "Are you sure you want to Import data for this spot?",
                        "Yes", "No"))
                ImportSpot();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportSpot()
        {
            if (Selection.gameObjects[0].TryGetComponent<BoxCollider2D>(out var collider))
                spot.SetTransformProperties(
                    collider.transform.position, collider.transform.eulerAngles, collider.size, collider.offset);
            else
                UnityEngine.Debug.LogWarning("You must lock the inspector and select an object with a BoxCollider2D Component!");
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayClearSpotButton()
        {
            if (spot.position != Vector3.zero)
            {
                if (GUILayout.Button("Clear Spot", GUILayout.Width(100)) &&
                    EditorUtility.DisplayDialog(
                        "Clear Spot",
                        "Are you sure you want to Clear data for this spot?",
                        "Yes", "No"))
                    ClearSpot();

                GUILayout.Label("Has Imported Values");
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearSpot()
            => spot.SetTransformProperties(Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpotAnswer()
        {
            GUILayout.Space(5);

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Is Answer: ", GUILayout.Width(70));

                spot.SetIsAnswer(EditorGUILayout.Toggle(spot.isAnswer));
            }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpotWrongDialogue()
        {
            if (!spot.isAnswer)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("On Wrong Dialogue: ", GUILayout.Width(120));

                    spot.SetWrongDialogue(HandyFields.UnityField(spot.wrongDialogue, 170, 25));
                }
            }
            else if (spot.isAnswer && spot.wrongDialogue != null)
                spot.SetWrongDialogue(null);

            GUILayout.Space(10);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayRemoveSpotButton(int i)
        {
            if (GUILayout.Button("Remove Spot", GUILayout.Width(100)) &&
                EditorUtility.DisplayDialog(
                    "Remove Spot",
                    "Are you sure you want to remove this spot?",
                    "Yes", "No"))
                builder.spots.RemoveAt(i);

            GUILayout.Space(25);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddSpotButton()
        {
            if (GUILayout.Button("Add Spot", GUILayout.Width(100)))
                builder.spots.Add(new Spot());
        }
    }
}
#endif