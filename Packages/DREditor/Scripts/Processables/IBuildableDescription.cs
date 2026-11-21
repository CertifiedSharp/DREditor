//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for a collection of descriptions that combine with a base description to combine into one full description.
    /// </summary>
    public interface IBuildableDescription
    {
        /// <summary>
        /// Combines a base description with a specified instances of base fragments into one full description.
        /// </summary>
        /// <param name="baseDescription">The base or default description for an object.</param>
        /// <param name="collectedFragments">Specified collected instances of base fragments to combine with the base description.</param>
        /// <returns>String description made up of the base description and collected instances of base fragments.</returns>
        string this[string baseDescription = "", List<string> collectedFragments = null] { get; }

        /// <summary>
        /// Retrieves a specific piece of a buildable description.
        /// </summary>
        /// <param name="index">The index position for the sought piece of the description.</param>
        /// <returns>The string piece of a buildable description.</returns>
        string this[int index] { get; }

        /// <summary>
        /// Retrieves the names or designations for a fragment.
        /// </summary>
        /// <returns>Array of strings that should contain the designation for a fragment.</returns>
        string[] GetFragmentNames();
    }
}