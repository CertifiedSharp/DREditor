//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Threading;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public static class ThreadSafeSimpleRandom
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly ThreadLocal<SimpleRandom> ThreadLocal = new(() =>
        {
            uint seed = (uint)(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId);

            return new SimpleRandom(seed);
        });

        /// <summary>
        ///
        /// </summary>
        public static SimpleRandom Instance => ThreadLocal.Value!;
    }
}