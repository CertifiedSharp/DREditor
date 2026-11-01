//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Processables
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class DescriptionFragmentCollection : IBuildableDescription
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        [SelectType]
        [Tooltip("")]
        public List<IDescriptionFragment> fragments = new();


        /// <inheritdoc/>
        public virtual bool IsValid() => fragments != null && fragments.Count > 0;


        /// <inheritdoc/>
        public virtual string this[string baseDescription, List<string> collectedFragments]
        {
            get
            {
                string description = "";

                description += baseDescription;

                if (IsValid())
                    foreach (IDescriptionFragment piece in fragments)
                        if (collectedFragments.Contains(piece.DisplayName))
                            description += piece.Description + "\n";

                return description;
            }
        }

        /// <inheritdoc/>
        public virtual string this[int index]
        {
            get
            {
                if (index == -1)
                    return "";

                if (fragments.Count > 0 && index >= 0 && index < fragments.Count)
                    return fragments[index].DisplayName;
                else
                    UnityEngine.Debug.LogError($"{ToString()}: " +
                        $"The fragment index {index} was not valid! Sending blank title." +
                        $"\nThe checked collection has a total of {fragments.Count} pieces.");

                return "";
            }
        }

        /// <inheritdoc/>
        public virtual string[] GetFragmentNames()
        {
            List<string> names = new();

            if (IsValid())
                foreach (IDescriptionFragment piece in fragments)
                    names.Add(piece.DisplayName);

            return names.ToArray();
        }
    }
}