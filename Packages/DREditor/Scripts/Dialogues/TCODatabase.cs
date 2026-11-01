//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Trials/Trial Camera Override Preset Database", fileName = "TCOPresets")]
    public class TCODatabase : ScriptableObject
    {
        public List<TCOPreset> Presets = new();


        public string[] GetNames()
        {
            List<string> names = new();

            foreach(TCOPreset p in Presets)
                names.Add(p.presetName);

            return names.ToArray();
        }

        public int[] GetInts()
        {
            int[] nums = new int[Presets.Count];

            for(int i = 0; i < Presets.Count; i++)
                nums[i] = i;

            return nums;
        }
    }
}
