//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR && CINEMACHINE
using Cinemachine;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.TrackPhrase
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(TrackPath))]
    public class TrackAnimationEditor : ClassEditorTypeBase<TrackPath>
    {
        /// <inheritdoc/>
        protected override void OnInspectorGUI() => PhraseTrack();

        /// <summary>
        ///
        /// </summary>
        protected virtual void PhraseTrack()
        {
            target.Position = HandyFields.Vector3Field("Track Position: ", target.Position, 200, 7);

            target.Rotation = HandyFields.Vector4Field("Track Angle: ", target.Rotation, 200, 7);

            DisplayTrackSpeed();

            GUILayout.Space(25);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTrackSpeed()
        {
            using (new GUILayout.HorizontalScope())
            {
                ImportPathSelection();

                DisplayClearWaypointsButton();

                target.SpeedCurve = EditorGUILayout.CurveField(target.SpeedCurve);

                DisplayResetCurveButton();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportPathSelection()
        {
            if (GUILayout.Button("Import Selection", GUILayout.Width(130)))
            {
                CinemachinePath.Waypoint[] points = Selection.gameObjects[0].GetComponent<CinemachinePath>().m_Waypoints;

                if (points != null && points.Length > 0)
                    target.Points = points;
                else
                    UnityEngine.Debug.Log("You need to select a Path in the hierarchy!");
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayClearWaypointsButton()
        {
            if (GUILayout.Button("Clear Waypoints", GUILayout.Width(130)))
                target.Points = null;

            if (target.SpeedCurve == null)
                ResetTrackCurve();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayResetCurveButton()
        {
            if (GUILayout.Button("Reset Curve", GUILayout.Width(100)) &&
                    EditorUtility.DisplayDialog("Reset Curve", "Are you sure you want to Reset this Curve?",
                        "Yes", "No"))
                ResetTrackCurve();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetTrackCurve()
        {
            target.SpeedCurve = new AnimationCurve()
            {
                keys = new Keyframe[] { new(0, 1), new(0.5f, 0.09f), new(1, 1) }
            };
        }
    }
}
#endif