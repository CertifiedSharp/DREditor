//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Trials/Minigame Type Database", fileName = "MinigameTypeDB")]
    public class MinigameTypeDB : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<MinigameType> minigameTypes = new();


        /// <summary>
        ///
        /// </summary>
        public string[] ToStringArray()
        {
            string[] stringArray = new string[minigameTypes.Count];

            for (int i = 0; i < minigameTypes.Count; i++)
                stringArray[i] = minigameTypes[i].TypeName;

            return stringArray;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int[] ToIntegerArray()
        {
            int[] intArray = new int[minigameTypes.Count];

            for (int i = 0; i < minigameTypes.Count; i++)
                intArray[i] = minigameTypes[i].TypeNumber;

            return intArray;
        }


#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnValidate()
        {
            int count = 0;

            foreach (MinigameType minigame in minigameTypes)
            {
                if (minigame != null)
                {
                    minigameTypes[count].SetTypeNumber(count);

                    count += 1;
                }
            }

            EditorUtility.SetDirty(this);
        }

#endif
    }
}