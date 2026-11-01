//DRPlaylist script Author: SeleniumSoul for DREditor
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class Music
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected string title;

        /// <summary>
        ///
        /// </summary>
        public string Title { set { title = value; } get { return title; } }

        /// <summary>
        ///
        /// </summary>
        public AudioClip BGM;
    }
}