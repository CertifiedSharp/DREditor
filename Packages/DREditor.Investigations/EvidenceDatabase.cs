//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Investigations
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Investigations/Evidence Database", fileName = "EvidenceDatabase")]
    public class EvidenceDatabase : ScriptableObject
    {
        public List<Evidence> Evidences = new();
    }
}