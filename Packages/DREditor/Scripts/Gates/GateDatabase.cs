//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Gates
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Gates/Database", fileName = "GateDatabase")]
    public class GateDatabase : ScriptableObject
    {
        public List<GateBase> Areas = new();

        public List<string> gatelessLocations = new() { "GYM_Start" };


        public Gate GetArea(string scene)
        {
            foreach (GateBase gate in Areas)
            {
                if (gate.IsGateFor(scene))
                    return (Gate)gate;
            }

            if (!gatelessLocations.Contains(scene))
                UnityEngine.Debug.LogWarning($"ALERT: Gateway could not be found for {scene}! " +
                    $"Make sure that the name spelling is correct and that the scene has been referenced in the unity builder!");

            return null;
        }

        public bool Contains(string scene)
        {
            foreach (GateBase a in Areas)
            {
                if (a != null && a.ToAreaName == scene)
                    return true;
            }

            return false;
        }

        public string[] GetGateNames()
        {
            List<string> names = new();

            foreach(GateBase g in Areas)
                names.Add(g.ToAreaName);

            return names.ToArray();
        }
    }
}

