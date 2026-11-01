//Author: Happy30
//Edited Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor.EventObjects
{

    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Event Objects/Transform", fileName = "New Transform")]
    public class TransformWithEvent : ValueWithEvent<PosRot, TransformEvent>
    {
        public bool Resolve()
        {
            return true;
        }

        public override void Init()
        {
            if (debug)
                UnityEngine.Debug.Log(name + "'s value Initialized to: " + InitialValue.Pos);

            base.Init();
        }

        public override void SetValue(PosRot x)
        {
            base.SetValue(x);

            if (debug)
                UnityEngine.Debug.Log(name + "'s value Initialized to: " + x.Pos);
        }
    }


    [Serializable]
    public class PosRot
    {
        public Vector3 Pos;
        public Quaternion Rot;

        public PosRot(Vector3 pos, Quaternion rot)
        {
            Pos = pos;
            Rot = rot;
        }

        public void Clear()
        {
            Pos = Vector3.zero;

            Rot = Quaternion.Euler(Vector3.zero);
        }
    }
}