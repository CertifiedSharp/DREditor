//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Progression/Chapter", fileName = "Chapter")]
    [MovedFrom(true, "DREditor.Progression")]
    public class Chapter : ScriptableObject
    {
        public string saveTitle;
        public string title;
        public List<Objective> Objectives = new List<Objective>();

        public string[] GetObjectives()
        {
            List<string> vs = new List<string>();
            foreach (Objective o in Objectives)
            {
                vs.Add(o.Description);
            }
            string[] x = vs.ToArray();
            return x;
        }
        public Objective GetObjective(string o)
        {
            foreach(Objective ob in Objectives)
            {
                if(ob.Description == o)
                {
                    return ob;
                }
            }
            return null;
        }

        public bool HasObjective(Objective sent)
        {
            foreach (Objective ob in Objectives)
                if (ob.Description == sent.Description)
                    return true;
            return false;
        }
    }
    [System.Serializable]
    [MovedFrom(true, "DREditor.Progression")]
    public class Objective
    {
        public string Description = "";
        public ScriptableObject ProgressionGate = null;
        public bool KeepInstanceData = false;
        public List<ScriptableObject> ProgressionGateways = new();
        public List<ProgressionFlag> Flags = new();

        public virtual void SetProgressionGate(ScriptableObject ProgressionGate) => this.ProgressionGate = ProgressionGate;

        public int GetRequiredFlagCount() => GetRequiredFlagList().Count;
        public List<ProgressionFlag> GetRequiredFlagList()
        {
            List<ProgressionFlag> tmpFlags = new List<ProgressionFlag>();
            foreach (ProgressionFlag flag in Flags)
            {
                if (!flag.optional)
                {
                    tmpFlags.Add(flag);
                }
            }
            return tmpFlags;
        }
        public string[] GetFlagNames()
        {
            string[] tmp = new string[Flags.Count];

            for(int i = 0; i < Flags.Count; i++)
            {
                tmp[i] = Flags[i].name;
            }

            return tmp;
        }
        public string GetOverrideFlag()
        {
            foreach(ProgressionFlag flag in Flags)
            {
                if(flag.toObjective != "")
                {
                    return flag.toObjective;
                }
            }
            return "";
        }
        public object Clone()
        {
            Objective o = new()
            {
                Description = Description,
                ProgressionGate = ProgressionGate,
                KeepInstanceData = KeepInstanceData
            };

            for (int i = 0; i < Flags.Count; i++)
            {
                o.Flags.Add((ProgressionFlag)Flags[i].Clone());
            }

            return o;
        }
    }

    /// <summary>
    ///
    /// </summary>
    [MovedFrom(true, "DREditor.Progression")]
    public interface IPersistentData
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="persistentData"></param>
        void CarryOver(IPersistentData persistentData);
    }
    [System.Serializable]
    [MovedFrom(true, "DREditor.Progression")]
    public class ProgressionFlag
    {
        public string name;
        public bool triggered;
        public bool optional = false;
        public string toObjective;

        public virtual void SetTriggered(bool triggered) => this.triggered = triggered;

        public ProgressionFlag() { }

        public ProgressionFlag(bool triggered) => this.triggered = triggered;

        public object Clone()
        {
            ProgressionFlag newFlag = new(triggered)
            {
                name = name,
                optional = optional,
                toObjective = toObjective
            };

            return newFlag;
        }
    }
}