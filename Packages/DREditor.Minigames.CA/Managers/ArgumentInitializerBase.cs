// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Managers
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ArgumentInitializerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        public abstract void InitializeArgument(ScriptableObject builder);
    }
}