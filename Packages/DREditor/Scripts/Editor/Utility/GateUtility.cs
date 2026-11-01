//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FPC;
using DREditor.Gates;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class GateUtility
    {
        [MenuItem("DREditor/Gates/Select Current Gate")]
        public static void SelectCurrentGate()
        {
            Gate gate = GetCurrentArea();

            if (CheckIf(gate != null, "A Gate for this scene could not be found."))
                Selection.activeObject = gate;
        }

        private static Gate GetCurrentArea()
        {
            string currentAreaName = SceneManager.GetActiveScene().name;

            var gateDatabase = BuilderUtility.SetAssetByType<GateDatabase>();

            return gateDatabase.GetArea(currentAreaName);
        }

        #region Set From Location
        [MenuItem("DREditor/Gates/Set Gate From Location")]
        public static void SetFromLocation()
        {
            using (new AssetDatabase.AssetEditingScope())
            {
                try
                {
                    var player = UnityEngine.Object.FindAnyObjectByType<MovePlayer>();

                    if (!CheckIf(player != null, failReason: "A Player could not be found to set a Gate From Location."))
                        return;

                    var holders = UnityEngine.Object.FindObjectsByType<MonoBehaviour>().OfType<IGateHolder>().ToArray().Cast<MonoBehaviour>();

                    Gate closestDoorGate = ((IGateHolder)GetClosestObject(player.transform.position, holders.ToArray())).GetSetGateway();

                    Gate currentArea = GateUtility.GetCurrentArea();

                    if (CheckIf(closestDoorGate != null, failReason: "The closest door to the player could not be found.") &&
                        CheckIf(currentArea != null, failReason: "A Gate for this scene could not be found."))
                        currentArea.fromLocations.Add(new(closestDoorGate, player.transform.position, player.transform.eulerAngles));
                    else
                        return;

                    EditorUtility.SetDirty(currentArea);

                    Selection.activeObject = currentArea;

                    UnityEngine.Debug.Log("Setting Gate Location Complete");
                }
                catch (Exception exception)
                {
                    UnityEngine.Debug.LogWarning("ALERT: An Error Occured during the process to set the Gate-From Location!");

                    UnityEngine.Debug.LogException(exception);
                }
            }
        }

        private static bool CheckIf(bool condition, string failReason)
        {
            if (!condition)
                UnityEngine.Debug.LogWarning("ALERT: " + failReason);

            return condition;
        }

        public static T GetClosestObject<T>(Vector3 currentPosition, T[] enemies) where T : MonoBehaviour
        {
            T bestTarget = null;

            float closestDistanceSqr = Mathf.Infinity;

            foreach (T potentialTarget in enemies)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;

                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;

                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }
        #endregion
    }
}