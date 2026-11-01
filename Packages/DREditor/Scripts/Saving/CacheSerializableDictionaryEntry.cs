//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    public static class CacheSerializableDictionaryEntry
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void CacheTypesFromRegisteredAsseblies() => SerializableDictionaryEntry.CacheTypesFromRegisteredAsseblies();
    }
}