//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Gates
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Gates/Gate", fileName = "Gateway")]
    public class Gate : ScriptableObject
    {
        [Tooltip("")]
        public string toAreaName;
        public string ToAreaName { get => toAreaName; set => toAreaName = value; }
        public List<InvGate> invGates = new();
        public bool isTPFD = false;

        [FormerlySerializedAs("fromLocationList")]
        public List<GateFrom> fromLocations = new(); // Thank you Leo For the reminder ;)
        public List<ChapterGate> chapterGates = new();
        public List<Waypoint> fastTravelMarkers = new();

        public GateFrom GetFromLocation(string roomName)
        {
            foreach (GateFrom from in fromLocations)
            {
                if (from.gateFrom.ToAreaName == roomName)
                    return from;
                foreach (InvGate g in from.gateFrom.invGates)
                    if (g.sceneName == roomName)
                        return from;
            }
            UnityEngine.Debug.Log("GateFrom not found in Gate " + ToAreaName);
            return null;
        }
        public GateFrom GetFromLocation(Gate gate)
        {
            var x = fromLocations.Where(n => n.gateFrom == gate);
            if (x.Count() > 0)
                return x.ElementAt(0);
            UnityEngine.Debug.Log("GateFrom not found in Gate");
            return null;
        }
        public string GetInvestigationRoom(int current)
        {
            foreach (InvGate invGate in invGates)
            {
                if (invGate.chapter == current)
                    return invGate.sceneName;
            }
            //UnityEngine.Debug.LogWarning("Investigation Room couldn't be found, returning to default area");
            return ToAreaName;
        }
        
        public string GetVariantScene(int chapter, string objective)
        {
            if (chapterGates.Count == 0)
                return ToAreaName;
            ChapterGate chapGate = GetChapterGate(chapter);
            if (chapGate == null)
                return ToAreaName;
            VariantGate variant = chapGate.GetVariantGate(objective);
            if (variant == null)
                return ToAreaName;
            
            return variant.sceneName;
        }
        ChapterGate GetChapterGate(int chapter)
        {
            foreach(ChapterGate chapterGate in chapterGates)
            {
                if(chapterGate.chapter == chapter) return chapterGate;
            }
            return null;
        }
        
    }
    [Serializable]
    public class GateFrom
    {
        public Gate gateFrom = null;
        public Vector3 position;
        public Vector3 rotation;

        public GateFrom() { }
        public GateFrom(Gate gateFrom, Vector3 position, Vector3 rotation)
        {
            this.gateFrom = gateFrom;
            this.position = position;
            this.rotation = rotation;
        }
    }
    [Serializable]
    public class Waypoint
    {
        public Vector3 position;
        public Vector3 rotation;
    }



    [Serializable]
    public class InvGate
    {
        public int chapter;
        public string sceneName;
    }

    [Serializable]
    public class ChapterGate
    {
        public int chapter = 0;
        public bool useDefault = false;
        [SerializeField] string defaultScene;
        public string GetDefaultScene() => defaultScene;
        public List<VariantGate> variants = new();
        public VariantGate GetVariantGate(string objective)
        {
            foreach (VariantGate variantGate in variants)
            {
                if (variantGate.objectiveName == objective) return variantGate;
            }
            return null;
        }
    }

    [Serializable]
    public class VariantGate
    {
        public string objectiveName;
        public string sceneName;
    }
}
