//DRPlaylist script Author: SeleniumSoul for DREditor
// Cleaned up by Sweden.
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Audio/Music Playlist", fileName = "New Music Playlist")]
    public class Playlist : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        public List<Music> Musics = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetAudioTitles()
        {
            string[] titles = new string[Musics.Count];

            for (int i = 0; i < Musics.Count; i++)
                titles[i] = Musics[i].Title;

            return titles;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int[] GetAudioCount()
        {
            int[] musiccount = new int[Musics.Count];

            for (int i = 0; i < Musics.Count; i++)
                musiccount[i] = i;

            return musiccount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public virtual AudioClip GetAudioClip(string title)
        {
            foreach (Music music in Musics)
                if (music.Title == title)
                    return music.BGM;

            UnityEngine.Debug.Log("Couldn't find song: " + title);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public virtual string GetTitleFromClip(AudioClip clip)
        {
            foreach (Music music in Musics)
                if (music.BGM == clip)
                    return music.Title;

            UnityEngine.Debug.Log("REPORT: Couldn't find Title: " + clip);

            return null;
        }
    }
}