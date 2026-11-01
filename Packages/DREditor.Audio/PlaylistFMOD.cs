//DRPlaylist script originally Author: SeleniumSoul for DREditor
// Edited by Sweden for FMOD Usage.
#if FMOD_UNITY
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Audio/Music Playlist FMOD", fileName = "New Music Playlist")]
    public class PlaylistFMOD : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        public List<MusicFMOD> Musics = new();

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public virtual string GetTitleFromClip(string clip)
        {
            foreach (MusicFMOD music in Musics)
                if (music.BGM == clip)
                    return music.Title;

            UnityEngine.Debug.Log("REPORT: Couldn't find Title: " + clip);

            return null;
        }
    }
}
#endif