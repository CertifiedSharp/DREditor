//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class VariableBase : EndEventBase
    {
        [FormerlySerializedAs("Chapter")]
        public int chapter = 0; //*
        public int Chapter { get => chapter; set => chapter = value; }

        [FormerlySerializedAs("Objective")]
        public int objective; //*
        public int Objective { get => objective; set => objective = value; }
        public List<int> Flags = new(); //*
        public List<int> FalseFlags = new(); //*
        //public List<bool> IntendedValues = new List<bool>(); //*
    }
}
