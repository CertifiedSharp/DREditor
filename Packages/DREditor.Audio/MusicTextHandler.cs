//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using TMPro;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class MusicTextHandler : MonoBehaviour 
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent setMusicText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener ClearMusicText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener ResetMusicText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text boxText = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => AttemptSubscribeToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptSubscribeToEvents()
        {
            if (boxText == null)
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(boxText)} has not been filled out on the MusicTextHandler, " +
                    "so it's functionality will not be used.");
            else
                SubscribeToEvents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
            setMusicText.AddListener(SetText);

            ClearMusicText.Response.AddListener(ClearText);

            ResetMusicText.Response.AddListener(ResetText);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToEvents()
        {
            setMusicText.RemoveListener(SetText);

            ClearMusicText.Response.RemoveListener(ClearText);

            ResetMusicText.Response.RemoveListener(ResetText);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        protected virtual void SetText(string text)
        {
            boxText.autoSizeTextContainer = false;

            boxText.text = text;

            boxText.autoSizeTextContainer = true;

            boxText.ForceMeshUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearText() => boxText.text = "";

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetText()
        {
            boxText.autoSizeTextContainer = false;

            ClearText();

            boxText.autoSizeTextContainer = true;

            boxText.ForceMeshUpdate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => UnsubscribeToEvents();
    }
}