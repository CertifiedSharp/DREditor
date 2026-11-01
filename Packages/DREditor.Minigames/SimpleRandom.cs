//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public class SimpleRandom
    {
        /// <summary>
        ///
        /// </summary>
        private uint seed;

        public SimpleRandom(uint initialSeed) => seed = initialSeed != 0 ? initialSeed : 1;

        /// <summary>
        ///
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public int Next(int max)
        {
            // LCG formula (killing myself)
            seed = 1664525 * seed + 1013904223; 

            return (int)(seed % max);
        }
    }
}