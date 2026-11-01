//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Investigations
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Investigations/Evidence", fileName = "Evidence")]
    public class Evidence : ScriptableObject
    {
        public List<TruthBullet> TruthBullets = new();

        public string[] ToStringArray()
        {
            string[] arr = new string[TruthBullets.Count];

            for (int i = 0; i < TruthBullets.Count; i++)
            {
                arr[i] = TruthBullets[i].DisplayName;
            }

            return arr;
        }

        public int[] TointArray()
        {
            int[] arr = new int[TruthBullets.Count];

            for (int i = 0; i < TruthBullets.Count; i++)
            {
                arr[i] = i;
            }

            return arr;
        }

        public virtual TruthBullet GetBullet(string text)
        {
            for (int i = 0; i < TruthBullets.Count; i++)
            {
                TruthBullet t = TruthBullets[i];

                if (text == t.DisplayName)
                    return t;
            }

            UnityEngine.Debug.LogWarning("Couldn't find TruthBullet: " + text + " !");

            return null;
        }
    }
}