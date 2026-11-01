//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class InputModifier : HittableModifierBase, IMistakeModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<InputModifierType> inputModifiers = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptableObject"></param>
        /// <returns></returns>
        public virtual bool Contains(ScriptableObject scriptableObject)
            => scriptableObject is InputModifierType type && inputModifiers.Contains(type);


        /// <inheritdoc/>
        public virtual bool UsedForMistake()
        {
            foreach (var modifier in inputModifiers)
                if (modifier.counterable)
                    return true;

            return false;
        }

        /// <inheritdoc/>
        public override string DisplayName
        {
            get
            {
                string inputs = "";

                foreach (var modifier in inputModifiers)
                    if (modifier != null)
                        inputs += inputModifiers.IndexOf(modifier) == inputModifiers.Count - 1 ? modifier.name : modifier.name + ", ";

                if (inputs == "")
                    return base.DisplayName;
                else
                    return base.DisplayName + ": " + inputs;
            }
        }
    }
}