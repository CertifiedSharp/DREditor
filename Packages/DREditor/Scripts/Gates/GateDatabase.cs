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
        public Gate ProgressionGate = null; // For when you need to only go to one area

        public List<Gate> Areas = new();

        public List<string> gatelessLocations = new() { "GYM_Start" };


        public Gate GetArea(string scene)
        {
            foreach (Gate a in Areas)
            {
                if (a.ToAreaName == scene)
                    return a;
                else if (a.chapterGates.Count != 0 && 
                    a.chapterGates.Where(x => x.variants.Where(y => y.sceneName == scene).Count() > 0).Count() > 0)
                    return a;
            }

            if (!gatelessLocations.Contains(scene))
                UnityEngine.Debug.LogWarning($"ALERT: Gateway could not be found for {scene}! " +
                    $"Make sure that the name spelling is correct and that the scene has been referenced in the unity builder!");

            return null;
        }

        public bool Contains(string scene)
        {
            foreach (Gate a in Areas)
            {
                if (a != null && a.ToAreaName == scene)
                    return true;
            }

            return false;
        }

        public string[] GetGateNames()
        {
            List<string> names = new();

            foreach(Gate g in Areas)
                names.Add(g.ToAreaName);

            return names.ToArray();
        }

        public bool IsInvGate(Gate g)
        {
            foreach(Gate gate in Areas)
            {
                if (gate.invGates.Where(n => n.sceneName == g.ToAreaName).ToArray().Length > 0)
                    return true;
            }

            return false;
        }

        public List<GateFrom> GetParentLocationList(Gate g)
        {
            foreach(Gate gate in Areas)
            {
                if (gate.invGates.Where(n => n.sceneName == g.ToAreaName).ToArray().Length > 0)
                    return gate.fromLocations;
            }

            return null;
        }
    }
}

