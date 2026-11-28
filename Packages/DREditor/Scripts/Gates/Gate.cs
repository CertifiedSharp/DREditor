//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace DREditor.Gates
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Gates/Gate", fileName = "Gateway")]
    public class Gate : GateBase, IDynamicCollection<IGateData>
    {
        [Tooltip("")]
        public string toAreaName;
        public override string ToAreaName { get => toAreaName; set => toAreaName = value; }

        public bool isTPFD = false;

        [FormerlySerializedAs("fromLocationList")]
        public List<GateFrom> fromLocations = new(); // Thank you Leo For the reminder ;)

        public List<Waypoint> fastTravelMarkers = new();

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SelectType]
        [SerializeReference] protected List<IGateData> gateDatas = new();


        public virtual List<IGateData> DynamicDatas => gateDatas;

        IEnumerable<object> IDynamicCollection.DynamicDatas => DynamicDatas;

        public virtual GateFrom GetFromLocation(string roomName)
        {
            foreach (GateFrom from in fromLocations)
                if (from.gateFrom.IsGateFor(roomName))
                    return from;

            UnityEngine.Debug.Log($"REPORT: GateFrom for {roomName} not found in Gate {ToAreaName}");

            return null;
        }

        public virtual GateFrom GetFromLocation(Gate gate)
        {
            if (fromLocations.Where(fromLocation => fromLocation.gateFrom == gate)?.First() is GateFrom gateFrom)
                return gateFrom;

            UnityEngine.Debug.Log($"REPORT: GateFrom for {gate.name} not found in {name}");

            return null;
        }
        
        public override bool IsGateFor(string scene)
        {
            if (base.IsGateFor(scene))
                return true;

            if (this.HasMany(out List<IGateIdentifiable> identifiables))
                foreach (var identifiable in identifiables)
                    if (identifiable.IsGateFor(scene))
                        return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objectiveName"></param>
        /// <returns></returns>
        public virtual string GetGateVariantDuring(int chapter, string objectiveName)
        {
            if (this.HasMany(out List<IGateVariant> variants))
                foreach (var variant in variants)
                    if (variant.IsVariantDuring(chapter, objectiveName, out string sceneName))
                        return sceneName;

            return null;
        }
    }

    [Serializable]
    public class GateFrom
    {
        public GateBase gateFrom = null;

        public Vector3 position;

        public Vector3 rotation;


        public GateFrom() { }

        public GateFrom(GateBase gateFrom, Vector3 position, Vector3 rotation)
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


    [MovedFrom(true, sourceClassName: "InvGate")]
    [Serializable]
    public class InvestigationGate : GateVariantBase
    {
        public int chapter;

        public string sceneName;



        public override bool IsGateFor(string scene) => sceneName == scene;


        public override bool IsVariantDuring(int chapter, string objectiveName, out string sceneName)
        {
            bool result = this.chapter == chapter;

            sceneName = result ? this.sceneName : null;

            return result;
        }
    }

    [Serializable]
    public class VariantGate : GateVariantBase
    {
        public int chapter = 0;

        public string objectiveName;

        public string sceneName;


        

        public override bool IsGateFor(string scene) => sceneName == scene;


        public override bool IsVariantDuring(int chapter, string objectiveName, out string sceneName) =>
            (sceneName = GetSceneFor(chapter, objectiveName)) != null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objectiveName"></param>
        /// <returns></returns>
        protected virtual string GetSceneFor(int chapter, string objectiveName) => 
            SceneNameValid() && ProgressionMatches(chapter, objectiveName) ? sceneName : null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objectiveName"></param>
        /// <returns></returns>
        protected virtual bool ProgressionMatches(int chapter, string objectiveName) => 
            this.chapter == chapter && this.objectiveName == objectiveName;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool SceneNameValid() => !string.IsNullOrEmpty(sceneName);
    }

    [Serializable]
    public abstract class GateVariantBase : IGateData, IGateVariant
    {
        public abstract bool IsGateFor(string scene);

        public abstract bool IsVariantDuring(int chapter, string objective, out string sceneName);
    }

    [Serializable]
    public abstract class GateBase : ScriptableObject, IGateIdentifiable
    {
        public abstract string ToAreaName { get; set; }


        public virtual bool IsGateFor(string scene) => ToAreaName == scene;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IGateIdentifiable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        bool IsGateFor(string scene);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IGateData : IGateIdentifiable
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public interface IGateVariant
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objectiveName"></param>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        bool IsVariantDuring(int chapter, string objectiveName, out string sceneName);
    }

    ///// <summary>
    ///// Interface for classes in the gate system to be converted.
    ///// </summary>
    //public interface IGateConvertable
    //{
    //    /// <summary>
    //    /// Copies over any matching type of data from the gate to the class implementing <see cref="IGateConvertable"/>.
    //    /// </summary>
    //    /// <param name="gate">The gate to get the old data from.</param>
    //    void Convert(Gate gate);
    //}
}
