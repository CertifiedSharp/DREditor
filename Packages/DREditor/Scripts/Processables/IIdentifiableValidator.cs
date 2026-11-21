//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for scripts that have the ability to validate integer ID's of objects.
    /// </summary>
    public interface IIdentifiableValidator
    {
        /// <summary>
        /// Checks whether the sent ID is valid.
        /// </summary>
        /// <param name="ID">Integer ID being validated.</param>
        /// <returns>True if the sent ID is valid.</returns>
        bool IDValid(int ID);
    }
}